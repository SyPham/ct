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
    public class ButtonService : IButtonService
    {
        private readonly IButtonRepository _repoBtn;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ButtonService(
        IButtonRepository repoBtn,
        IMapper mapper, 
        MapperConfiguration configMapper)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoBtn = repoBtn;

        }

        //Thêm Brand mới vào bảng Line
        public async Task<bool> Add(ButtonDto model)
        {
            var Line = _mapper.Map<Button>(model);
            _repoBtn.Add(Line);
            return await _repoBtn.SaveAll();
        }



        //Lấy danh sách Brand và phân trang
        public async Task<PagedList<ButtonDto>> GetWithPaginations(PaginationParams param)
        {
            var lists = _repoBtn.FindAll().ProjectTo<ButtonDto>(_configMapper).OrderByDescending(x => x.ID);
            return await PagedList<ButtonDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        //Tìm kiếm Line
        public async Task<PagedList<ButtonDto>> Search(PaginationParams param, object text)
        {
            var lists = _repoBtn.FindAll().ProjectTo<ButtonDto>(_configMapper)
            .Where(x => x.Name.Contains(text.ToString()))
            .OrderByDescending(x => x.ID);
            return await PagedList<ButtonDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }
        //Xóa Brand
        public async Task<bool> Delete(object id)
        {
            var Line = _repoBtn.FindById(id);
            _repoBtn.Remove(Line);
            return await _repoBtn.SaveAll();
        }

        //Cập nhật Brand
        public async Task<bool> Update(ButtonDto model)
        {
            var btn = _mapper.Map<Button>(model);
            _repoBtn.Update(btn);
            return await _repoBtn.SaveAll();
        }

        //Lấy toàn bộ danh sách Brand 
        public async Task<List<ButtonDto>> GetAllAsync()
        {
            return await _repoBtn.FindAll().ProjectTo<ButtonDto>(_configMapper).OrderByDescending(x => x.ID).ToListAsync();
        }

        //Lấy Brand theo Brand_Id
        public ButtonDto GetById(object id)
        {
            return _mapper.Map<Button, ButtonDto>(_repoBtn.FindById(id));
        }

       
        public async Task<List<ButtonDto>> GetAllButtonByWorkerID(int workerID)
        {
            return (await _repoBtn.FindAll().OrderByDescending(x => x.ID).ToListAsync()).Select(x => new ButtonDto
            {
                ID = x.ID,
                Name = x.Name,
                Code = x.Code,
                Standard = x.Standard,
                WorkerID = x.WorkerID,
                HasExist = x.WorkerID != null ? true : false,
                Status = x.WorkerID == workerID ? true : false
            }).ToList();
        }

        public async Task<bool> CheckExistWorkerLinkButton(int workerID)
        {
            var worker = await _repoBtn.FindAll().FirstOrDefaultAsync(x => x.WorkerID == workerID);

            return worker != null ? true : false;
        }
        public async Task<bool> CheckExistButtonLinkWorker(int btn)
        {
            var button = await _repoBtn.FindAll().FirstOrDefaultAsync(x => x.ID == btn);

            return button.WorkerID != null ? true : false;
        }
        public async Task<bool> UnlinkButtonLinkWorker(int btn)
        {
            var model = await _repoBtn.FindAll().FirstOrDefaultAsync(x => x.ID == btn);
            if (model == null) return true;
            model.WorkerID = null;
            _repoBtn.Update(model);
            return await _repoBtn.SaveAll();
        }
        public async Task<bool> UnlinkWorkerWithButton( int workerID)
        {
            var model = await _repoBtn.FindAll().FirstOrDefaultAsync(x => x.WorkerID == workerID);
            if (model == null) return true;
            model.WorkerID = null;
            _repoBtn.Update(model);
            return await _repoBtn.SaveAll();
        }
    }
}