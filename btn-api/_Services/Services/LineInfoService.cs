using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btn_api.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using btn_api._Repositories.Interface;
using btn_api._Services.Interface;
using btn_api.DTO;
using btn_api.Models;
using Microsoft.EntityFrameworkCore;

namespace btn_api._Services.Services
{
    public class LineInfoService : ILineInfoService
    {
        private readonly ILineInfoRepository _repoLine;
        private readonly IBuildingRepository _repoBuilding;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public LineInfoService(ILineInfoRepository repoBrand,
        IBuildingRepository repoBuilding,
         IMapper mapper, MapperConfiguration configMapper)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoLine = repoBrand;
            _repoBuilding = repoBuilding;

        }

        //Thêm Brand mới vào bảng Line
        public async Task<bool> Add(LineInfoDto model)
        {
            var info = _mapper.Map<LineInfo>(model);
            info.LineID = _repoBuilding.FindAll().FirstOrDefault(x=> x.Name.Equals(model.LineName)).ID;
            _repoLine.Add(info);
            return await _repoLine.SaveAll();
        }

     

        //Lấy danh sách Brand và phân trang
        public async Task<PagedList<LineInfoDto>> GetWithPaginations(PaginationParams param)
        {
            var lists = _repoLine.FindAll().ProjectTo<LineInfoDto>(_configMapper).OrderByDescending(x => x.ID);
            return await PagedList<LineInfoDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }
      
        //Tìm kiếm Line
        public async Task<PagedList<LineInfoDto>> Search(PaginationParams param, object text)
        {
            var lists = _repoLine.FindAll().ProjectTo<LineInfoDto>(_configMapper)
            .Where(x => x.ModelName.Contains(text.ToString()))
            .OrderByDescending(x => x.ID);
            return await PagedList<LineInfoDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }
        //Xóa Brand
        public async Task<bool> Delete(object id)
        {
            var Line = _repoLine.FindById(id);
            _repoLine.Remove(Line);
            return await _repoLine.SaveAll();
        }

        //Cập nhật Brand
        public async Task<bool> Update(LineInfoDto model)
        {
            var info = _mapper.Map<LineInfo>(model);
            info.LineID = _repoBuilding.FindAll().FirstOrDefault(x => x.Name.Equals(model.LineName)).ID;
            _repoLine.Update(info);
            return await _repoLine.SaveAll();
        }
      
        //Lấy toàn bộ danh sách Brand 
        public async Task<List<LineInfoDto>> GetAllAsync()
        {
            return await _repoLine.FindAll().ProjectTo<LineInfoDto>(_configMapper).OrderByDescending(x => x.ID).ToListAsync();
        }

        //Lấy Brand theo Brand_Id
        public LineInfoDto GetById(object id)
        {
            return  _mapper.Map<LineInfo, LineInfoDto>(_repoLine.FindById(id));
        }

        public async Task<LineInfoDto> GetLineInfoByLine(string lineName)
        {
           return  _mapper.Map<LineInfo, LineInfoDto>(await _repoLine.FindAll().Include(x=>x.Building).FirstOrDefaultAsync(x=> x.Building.Name == lineName));
        }
    }
}