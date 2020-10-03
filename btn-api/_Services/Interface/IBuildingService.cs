using btn_api.DTO;
using btn_api.Helpers;
using btn_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Services.Interface
{
   public interface IBuildingService : IECService<BuildingDto>
    {
        Task<IEnumerable<HierarchyNode<BuildingDto>>> GetAllAsTreeView();
        Task<List<BuildingDto>> GetBuildings();
        Task<List<BuildingDto>> GetLines();
        Task<object> GetBuildingsForSetting();
        Task<object> CreateMainBuilding(BuildingDto buildingDto);
        Task<object> CreateSubBuilding(BuildingDto buildingDto);
        Task<object> GetBuildingAsTreeView();
    }
}
