using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btn_api.Models;

namespace btn_api.DTO
{
    public class WorkerDto
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string WorkerCode { get; set; }
        public string Operation { get; set; }     
        public int? BuildingID { get; set; }
        public Building Building { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class WorkerListDto
    {
        public int WorkerID { get; set; }
        public int LineID { get; set; }
        public int ButtonCode { get; set; }
        public int ButtonID { get; set; }
        public string FullName { get; set; }
        public string Operation { get; set; }
        public int TaktTime { get; set; }
        public double Best { get; set; }
        public double Latest { get; set; }
        public int Frequency { get; set; }
        public double Avg { get; set; }
        public string LineName { get; set; }
        public int Standard { get; set; }
    }
    public class ChartDto
    {
        public List<double> data { get; set; }
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string pointBackgroundColor { get; set; }
    }
    public class OperationDto
    {
        public int WorkerID { get; set; }
        public string Operation { get; set; }
    }
    public class TaktTimeDto
    {
        public int ButtonID { get; set; }
        public int TaktTime { get; set; }
    }
    public class StandardDto
    {
        public int ButtonID { get; set; }
        public int Standard { get; set; }
    }
    public class ExcelExportDto
    {
        public string PressTime { get; set; }
        public double CT { get; set; }
        public int TT { get; set; }
        public int Standard { get; set; }
        public double AVG { get; set; }

    }
    public class ExcelExportForChartDto
    {
        public HeaderDto Header1 { get; set; }
        public HeaderDto Header2 { get; set; }
        public HeaderForDataDto HeaderForDataDto { get; set; }
        public List<ExcelExportDto> DataExcelExportDtos { get; set; }

    }
    public class HeaderDto
    {
        public string Type { get; set; }
        public string Value { get; set; }

    }
    public class HeaderForDataDto
    {
        public string PressTime { get; set; }
        public string CT { get; set; }
        public string TT { get; set; }
        public string Standard { get; set; }
        public string AVG { get; set; }

    }
}
