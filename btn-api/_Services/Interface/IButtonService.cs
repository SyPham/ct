using btn_api.DTO;
using btn_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Services.Interface
{
    public interface IButtonService : IECService<ButtonDto>
    {
        Task<List<ButtonDto>> GetAllButtonByWorkerID(int workerID);
        Task<bool> CheckExistWorkerLinkButton(int workerID);
        Task<bool> UnlinkWorkerWithButton(int workerID);
        Task<bool> UnlinkButtonLinkWorker(int btn);
        Task<bool> CheckExistButtonLinkWorker(int btn);
    }
}
