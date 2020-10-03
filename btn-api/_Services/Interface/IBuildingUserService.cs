using btn_api.DTO;
using btn_api.Helpers;
using btn_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Services.Interface
{
   public interface IBuildingUserService : IECService<BuildingUserDto>
    {
        Task<object> MappingUserWithBuilding(BuildingUserDto buildingUserDto);
        Task<object> RemoveBuildingUser(BuildingUserDto buildingUserDto);
        Task<List<BuildingUserDto>> GetBuildingUserByBuildingID(int buildingID);
        Task<object> GetBuildingByUserID(int userid);
        Task<object> MapBuildingUser(int userid, int buildingid);
    }
}
