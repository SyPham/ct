using btn_api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Services.Interface
{
    public interface IWorkerService : IECService<WorkerDto>
    {
        Task<object> GetWorkersByLine(string lineID);
        Task<object> Chart(int workerID, int btnID);
        Task<bool> UpdateOperation(OperationDto operationDto);
        Task<bool> UpdateTaktTime(TaktTimeDto taktTimeDto);
        Task<bool> UpdateStandard(StandardDto standardDto);
        Task<ExcelExportForChartDto> ExcelExport(int workerID, int btnID);
    }
}
