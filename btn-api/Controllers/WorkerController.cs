using System;
using System.Security.Claims;
using System.Threading.Tasks;
using btn_api.Helpers;
using btn_api._Services.Interface;
using btn_api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using System.Linq;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using OfficeOpenXml.Drawing.Chart;
using System.Drawing;
using OfficeOpenXml.Drawing.Chart.Style;
using OfficeOpenXml.Drawing;

namespace btn_api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        public WorkerController(IWorkerService partService)
        {
            _workerService = partService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetLines([FromQuery]PaginationParams param)
        //{
        //    var lines = await _lineService.GetWithPaginations(param);
        //    Response.AddPagination(lines.CurrentPage,lines.PageSize,lines.TotalCount,lines.TotalPages);
        //    return Ok(lines);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lines = await _workerService.GetAllAsync();
            return Ok(lines);
        }

        //[HttpGet("{text}")]
        //public async Task<IActionResult> Search([FromQuery]PaginationParams param, string text)
        //{
        //    var lists = await _lineService.Search(param, text);
        //    Response.AddPagination(lists.CurrentPage, lists.PageSize, lists.TotalCount, lists.TotalPages);
        //    return Ok(lists);
        //}
       
        [HttpPost]
        public async Task<IActionResult> Create(WorkerDto create)
        {

            if (_workerService.GetById(create.ID) != null)
                return BadRequest("Worker ID already exists!");
            //create.CreatedDate = DateTime.Now;
            if (await _workerService.Add(create))
            {
                return NoContent();
            }

            throw new Exception("Creating the part failed on save");
        }

        [HttpPut]
        public async Task<IActionResult> Update(WorkerDto update)
        {
            if (await _workerService.Update(update))
                return NoContent();
            return BadRequest($"Updating Worker {update.ID} failed on save");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOperation(OperationDto update)
        {
            if (await _workerService.UpdateOperation(update))
                return NoContent();
            return BadRequest($"Updating Worker {update.WorkerID} failed on save");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStandard(StandardDto update)
        {
            if (await _workerService.UpdateStandard(update))
                return NoContent();
            return BadRequest($"Updating button {update.ButtonID} failed on save");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateTaktTime(TaktTimeDto update)
        {
            if (await _workerService.UpdateTaktTime(update))
                return NoContent();
            return BadRequest($"Updating button {update.ButtonID} failed on save");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _workerService.Delete(id))
                return NoContent();
            throw new Exception("Error deleting the Worker");
        }
        [HttpGet("{lineID}")]
        public async Task<IActionResult> GetWorkersByLine(string lineID)
        {
            return Ok(await _workerService.GetWorkersByLine(lineID));
        }
        [HttpGet("{workerID}/{btnID}")]
        public async Task<IActionResult> Chart(int workerID, int btnID)
        {
            return Ok(await _workerService.Chart(workerID, btnID));
        }
        [HttpGet]
        public IActionResult ExcelExport2()
        {
            //create a new ExcelPackage
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create a WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                //fill cell data with a loop, note that row and column indexes start at 1
                Random rnd = new Random();
                for (int i = 2; i <= 11; i++)
                {
                    worksheet.Cells[1, i].Value = "Value " + (i - 1);
                    worksheet.Cells[2, i].Value = rnd.Next(5, 25);
                    worksheet.Cells[3, i].Value = rnd.Next(5, 25);
                }
                worksheet.Cells[2, 1].Value = "Age 1";
                worksheet.Cells[3, 1].Value = "Age 2";

                //create a new piechart of type Line
                ExcelLineChart lineChart = worksheet.Drawings.AddChart("lineChart", eChartType.Line) as ExcelLineChart;

                //set the title
                lineChart.Title.Text = "LineChart Example";

                //create the ranges for the chart
                var rangeLabel = worksheet.Cells["B1:K1"];
                var range1 = worksheet.Cells["B2:K2"];
                var range2 = worksheet.Cells["B3:K3"];

                //add the ranges to the chart
                lineChart.Series.Add(range1, rangeLabel);
                lineChart.Series.Add(range2, rangeLabel);

                //set the names of the legend
                lineChart.Series[0].Header = worksheet.Cells["A2"].Value.ToString();
                lineChart.Series[1].Header = worksheet.Cells["A3"].Value.ToString();

                //position of the legend
                lineChart.Legend.Position = eLegendPosition.Right;

                //size of the chart
                lineChart.SetSize(600, 300);

                //add the chart at cell B6
                lineChart.SetPosition(5, 0, 1, 0);
                Byte[] bin = excelPackage.GetAsByteArray();
                return File(bin, "application/octet-stream", "CycleTimeData.xlsx");
            }
        }
        [HttpGet("{workerID}/{btnID}")]
        [Obsolete]
        public async Task<IActionResult> ExcelExport(int workerID, int btnID)
        {
            var data = await _workerService.ExcelExport(workerID, btnID);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var memoryStream = new MemoryStream();
                using (ExcelPackage p = new ExcelPackage(memoryStream))
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "Henry Pham";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "Cycle Time Data";
                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("Cycle Time Data");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets["Cycle Time Data"];

                    // đặt tên cho sheet
                    ws.Name = "Cycle Time Data";
                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Calibri";

                    ws.Cells[1, 1].Value = data.Header1.Type;
                    ws.Cells[1, 2].Value = data.Header1.Value;

                    ws.Cells[2, 1].Value = data.Header2.Type;
                    ws.Cells[2, 2].Value = data.Header2.Value;

                    ws.Cells[3, 1].Value = data.HeaderForDataDto.PressTime;
                    ws.Cells[3, 2].Value = data.HeaderForDataDto.CT;
                    ws.Cells[3, 3].Value = data.HeaderForDataDto.TT;
                    ws.Cells[3, 4].Value = data.HeaderForDataDto.Standard;
                    ws.Cells[3, 5].Value = data.HeaderForDataDto.AVG;

                    int colIndex = 1;
                    int rowIndex = 3;
                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var item in data.DataExcelExportDtos)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;

                        //gán giá trị cho từng cell                      
                        ws.Cells[rowIndex, colIndex++].Value = item.PressTime;
                        ws.Cells[rowIndex, colIndex++].Value = item.CT;
                        ws.Cells[rowIndex, colIndex++].Value = item.TT;
                        ws.Cells[rowIndex, colIndex++].Value = item.Standard;
                        ws.Cells[rowIndex, colIndex++].Value = item.AVG;

                    }
                    //create a new piechart of type Line
                    ExcelLineChart lineChart = ws.Drawings.AddChart("lineChart", eChartType.Line) as ExcelLineChart;
                    lineChart.Style = eChartStyle.Style48;
                    // lineChart.Style = eChartStyle.Style30;
                    //set the title
                    lineChart.Title.Text = $"Cycle Time Data - {data.Header1.Value}";
                    //create the ranges for the chart
                    var rangeLabel = ws.Cells[4,1, rowIndex, 1];
                    var rangeCT = ws.Cells[4, 2, rowIndex, 2];
                    var rangeTT = ws.Cells[4, 3, rowIndex, 3];
                    var rangeStandard = ws.Cells[4, 4, rowIndex, 4];
                    var rangeAVG = ws.Cells[4, 5, rowIndex, 5];


                    //add the ranges to the chart
                    lineChart.Series.Add(rangeCT, rangeLabel);
                    lineChart.Series.Add(rangeTT, rangeLabel);
                    lineChart.Series.Add(rangeStandard, rangeLabel);
                    lineChart.Series.Add(rangeAVG, rangeLabel);

                    //set the names of the legend
                     lineChart.Series[0].Header = "Cycle Time";
                     lineChart.Series[1].Header = "TAKT Time";
                     lineChart.Series[2].Header = "Standard";
                    lineChart.Series[3].Header = "Average";
                    Color ctColor = (Color)ColorTranslator.FromHtml("orange");
                    Color ttColor = (Color)ColorTranslator.FromHtml("#808080");
                    Color stdColor = (Color)ColorTranslator.FromHtml("#36a2eb");
                    Color avgColor = (Color)ColorTranslator.FromHtml("#4bc0c0");

                    lineChart.Series[0].MarkerLineColor = ctColor;
                    lineChart.Series[1].MarkerLineColor = ttColor;
                    lineChart.Series[2].MarkerLineColor = stdColor;
                    lineChart.Series[3].MarkerLineColor = avgColor;

                    lineChart.Series[0].LineColor = ctColor;
                    lineChart.Series[1].LineColor = ttColor;
                    lineChart.Series[2].LineColor = stdColor;
                    lineChart.Series[3].LineColor = avgColor;
                    //position of the legend
                    lineChart.Legend.Position = eLegendPosition.Top;
                    //size of the chart
                    lineChart.SetSize(700, 350);
                    //add the chart at cell B6
                    lineChart.SetPosition(1, 0, 6, 0);
                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    return File(bin, "application/octet-stream", "CycleTimeData.xlsx");
                }
            }
            catch(Exception ex)
            {
                var mes = ex.Message;
                Console.Write(mes);
                return NotFound();
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/octet-stream"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
   
    }
}