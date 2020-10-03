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
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _repoWorker;
        private readonly ICycleTimeRepository _repoCycleTime;
        private readonly IBuildingRepository _repoLine;
        private readonly IButtonRepository _repoBtn;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public WorkerService(IWorkerRepository repoWorker,
        IBuildingRepository repoLine,
        IButtonRepository repoBtn,
        ICycleTimeRepository repoCycleTime,
        IMapper mapper, MapperConfiguration configMapper)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoWorker = repoWorker;
            _repoLine = repoLine;
            _repoBtn = repoBtn;
            _repoCycleTime = repoCycleTime;
        }

        //Thêm Brand mới vào bảng Line
        public async Task<bool> Add(WorkerDto model)
        {
            var Line = _mapper.Map<Worker>(model);
            _repoWorker.Add(Line);
            return await _repoWorker.SaveAll();
        }



        //Lấy danh sách Brand và phân trang
        public async Task<PagedList<WorkerDto>> GetWithPaginations(PaginationParams param)
        {
            var lists = _repoWorker.FindAll().ProjectTo<WorkerDto>(_configMapper).OrderByDescending(x => x.ID);
            return await PagedList<WorkerDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }

        //Tìm kiếm Line
        public async Task<PagedList<WorkerDto>> Search(PaginationParams param, object text)
        {
            var lists = _repoWorker.FindAll().ProjectTo<WorkerDto>(_configMapper)
            .Where(x => x.FullName.Contains(text.ToString()))
            .OrderByDescending(x => x.ID);
            return await PagedList<WorkerDto>.CreateAsync(lists, param.PageNumber, param.PageSize);
        }
        //Xóa Brand
        public async Task<bool> Delete(object id)
        {
            var Line = _repoWorker.FindById(id);
            _repoWorker.Remove(Line);
            return await _repoWorker.SaveAll();
        }

        //Cập nhật Brand
        public async Task<bool> Update(WorkerDto model)
        {
            var Line = _mapper.Map<Worker>(model);
            _repoWorker.Update(Line);
            return await _repoWorker.SaveAll();
        }

        //Lấy toàn bộ danh sách Brand 
        public async Task<List<WorkerDto>> GetAllAsync()
        {
            return await _repoWorker.FindAll().ProjectTo<WorkerDto>(_configMapper).OrderByDescending(x => x.ID).ToListAsync();
        }

        //Lấy Brand theo Brand_Id
        public WorkerDto GetById(object id)
        {
            return _mapper.Map<Worker, WorkerDto>(_repoWorker.FindById(id));
        }

        public async Task<object> GetWorkersByLine(string lineID)
        {
            try
            {
                var iotModel = await _repoCycleTime.FindAll().Where(x => x.datetime.Date == DateTime.Now.Date &&  x.cycleTime <= 90).ToListAsync();
                var WORKER_LEVEL = 5;
                var model = await (from a in _repoWorker.FindAll()
                                   join b in _repoLine.FindAll().Where(x => x.Level == WORKER_LEVEL && x.Name == lineID) on a.BuildingID equals b.ID
                                   join c in _repoBtn.FindAll() on a.ID equals c.WorkerID
                                   select new WorkerListDto
                                   {
                                       FullName = a.FullName,
                                       WorkerID = a.ID,
                                       LineID = b.ID,
                                       ButtonCode = c.Code,
                                       ButtonID = c.ID,
                                       Standard = c.Standard,
                                       LineName = b.Name,
                                       Operation = a.Operation,
                                       TaktTime = c.TaktTime
                                   }).ToListAsync();
                           var data = new List<WorkerListDto>();        
                foreach (var a in model)
                {
                      var avg =  iotModel.Where(x => x.BtnID == a.ButtonID).Count() > 0 ? iotModel.Where(x => x.BtnID == a.ButtonID).Average(x => x.cycleTime) : 0;
                      var  best =  iotModel.Where(x => x.BtnID == a.ButtonID).Count() > 0 ? iotModel.Where(x => x.BtnID == a.ButtonID).Min(x => x.cycleTime) : 0;
                       var latest =  iotModel.Where(x => x.BtnID == a.ButtonID).LastOrDefault() == null ? 0 : iotModel.Where(x => x.BtnID == a.ButtonID).LastOrDefault().cycleTime;
                       var frequency = iotModel.Where(x => x.BtnID == a.ButtonID).Count();
                    data.Add(new WorkerListDto
                    {
                        FullName = a.FullName,
                        WorkerID = a.WorkerID,
                        LineID = a.LineID,
                        ButtonCode = a.ButtonCode,
                        ButtonID = a.ButtonID,
                        Standard = a.Standard,
                        LineName = a.LineName,
                        Operation = a.Operation,
                        TaktTime =a.TaktTime,
                        Avg= Math.Round(avg, 1),
                        Best= best,
                        Latest = latest,
                        Frequency = frequency
                    });
                }
               
                return new
                {
                    data = data,
                    dataIoT = iotModel

                };
            }
            catch
            {
                return new
                {
                    data = new List<WorkerListDto>(),
                    dataIoT = new List<WorkerListDto>(),

                };

            }

        }

        public async Task<object> Chart(int workerID, int btnID)
        {
            try
            {
                var iotModel = await _repoCycleTime.FindAll().Where(x => x.datetime.Date == DateTime.Now.Date && x.BtnID == btnID &&  x.cycleTime <= 90).ToListAsync();
                var button = await _repoBtn.FindAll().Include(x => x.Worker).FirstOrDefaultAsync(x => x.WorkerID == workerID);
                var avg = new ChartDto
                {
                    label = "AVG",
                    data = new List<double> { Math.Round(iotModel.Average(x => x.cycleTime), 1) }
                };
                var best = new ChartDto
                {
                    label = "Best",
                    data = new List<double> { iotModel.Min(x => x.cycleTime) }
                };
                var standard = new ChartDto
                {
                    label = "Standard",
                    data = new List<double> { button.Standard },
                };
                var current = new ChartDto
                {
                    label = "Latest",
                    data = new List<double> { iotModel.OrderByDescending(x=>x.datetime).FirstOrDefault().cycleTime },
                };
                var tt = new ChartDto
                {
                    label = "TT",
                    data = new List<double> { button.TaktTime },
                };
                var chartData = new List<ChartDto>{
                    tt, standard, avg, best, current
                };
                return new
                {
                    chartData,
                    button
                };
            }
            catch
            {
                return new
                {
                    chartData = new object[] { },
                    button = new { },
                };
            }
        }

        public async Task<bool> UpdateOperation(OperationDto operationDto)
        {
            var item = _repoWorker.FindById(operationDto.WorkerID);
            item.Operation = operationDto.Operation;
            _repoWorker.Update(item);
            return await _repoWorker.SaveAll();
        }

        public async Task<ExcelExportForChartDto> ExcelExport(int workerID, int btnID)
        {
            var iotModel = await _repoCycleTime.FindAll().Where(x => x.datetime.Date == DateTime.Now.Date && x.BtnID == btnID && x.cycleTime > 0 && x.cycleTime <= 90).OrderBy(x=>x.datetime).ToArrayAsync();
            var button = await _repoBtn.FindAll().Include(x => x.Worker).FirstOrDefaultAsync(x => x.WorkerID == workerID);
            var header = new HeaderDto {
                Type = "FullName",
                Value = button.Worker.FullName
            };
            var header2 = new HeaderDto
            {
                Type = "Operation",
                Value = button.Worker.Operation
            };
            var header3 = new HeaderForDataDto
            {
                PressTime = "Press Time",
                CT = "Cycle Time",
                TT = "TAKT Time",
                Standard = "Standard",
                AVG = "AVG"
            };
            var result  = new ExcelExportForChartDto();
            var list = new List<ExcelExportDto>();
            var length = iotModel.Length;
            for (int i = 0; i <= length - 1; i++)
            {
                var item1 = iotModel[i].datetime;
                var index2 = i + 1;
                if (i == 0)
                {
                    // list.Add(new ExcelExportDto { PressTime = item1.ToString("HH:mm:ss")});
                }
                else if (index2 == length)
                {
                    // list.Add(new ExcelExportDto { PressTime = item1.ToString("HH:mm:ss")});
                }
                else
                {
                    var item2 = iotModel[i + 1].datetime;
                    var cycleTime = item2 - item1;
                    double avg = 0;
                    if (list.Count == 0) {
                        avg = cycleTime.TotalSeconds;
                    } else {
                        avg = Math.Round((list.Sum(x => x.CT) + cycleTime.TotalSeconds) / i, 1);
                    }
                    list.Add(new ExcelExportDto { 
                        PressTime = item1.ToString("HH:mm:ss"), 
                        CT = cycleTime.TotalSeconds,
                        TT = button.TaktTime,
                        Standard = button.Standard,
                        AVG = avg
                    });
                }
            }
            result.Header1 = header;
            result.Header2 = header2;
            result.HeaderForDataDto = header3;
            result.DataExcelExportDtos = list;
            return result;
        }

        public async Task<bool> UpdateStandard(StandardDto standardDto)
        {
            var item = _repoBtn.FindById(standardDto.ButtonID);
            item.Standard = standardDto.Standard;
            _repoBtn.Update(item);
            return await _repoBtn.SaveAll();
        }

        public async Task<bool> UpdateTaktTime(TaktTimeDto taktTimeDto)
        {
            var item = _repoBtn.FindById(taktTimeDto.ButtonID);
            item.TaktTime = taktTimeDto.TaktTime;
            _repoBtn.Update(item);
            return await _repoBtn.SaveAll();
        }
    }
}