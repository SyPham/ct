using btn_api.DTO;
using btn_api.Helpers;
using btn_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Services.Interface
{
   public interface IBuildingWorkerService : IECService<BuildingWorkerDto>
    {
        Task<object> MappingUserWithBuilding(BuildingWorkerDto buildingUserDto);
        Task<object> RemoveBuildingWorker(BuildingWorkerDto buildingUserDto);
        Task<List<BuildingWorkerDto>> GetBuildingWorkerByBuildingID(int buildingID);
        Task<object> GetBuildingByWorkerID(int workerId);
        Task<object> MapBuildingWorker(int workerId, int buildingid);
    }
}
