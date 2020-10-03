using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btn_api._Services.Interface;
using btn_api.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace btn_api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BuildingWorkerController : ControllerBase
    {
        private readonly IBuildingWorkerService _buildingUserService;

        public BuildingWorkerController(IBuildingWorkerService buildingUserService)
        {
            _buildingUserService = buildingUserService;
        }
        [HttpPost]
        public async Task<IActionResult> MappingUserWithBuilding([FromBody]BuildingWorkerDto buildingUserDto)
        {
            var result = await _buildingUserService.MappingUserWithBuilding(buildingUserDto);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveBuildingWorker([FromBody] BuildingWorkerDto buildingUserDto)
        {
            var result = await _buildingUserService.RemoveBuildingWorker(buildingUserDto);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBuildingWorkers()
        {
            var result = await _buildingUserService.GetAllAsync();
            return Ok(result);
        }
        
        [HttpGet("{buildingID}")]
        public async Task<IActionResult> GetBuildingWorkerByBuildingID(int buildingID)
        {
            var result = await _buildingUserService.GetBuildingWorkerByBuildingID(buildingID);
            return Ok(result);
        }
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetBuildingByWorkerID(int userID)
        {
            var result = await _buildingUserService.GetBuildingByWorkerID(userID);
            return Ok(result);
        }
        [HttpGet("{userid}/{buildingid}")]
        public async Task<IActionResult> MapBuildingWorker(int userid, int buildingid)
        {
            var result = await _buildingUserService.MapBuildingWorker(userid, buildingid);
                return Ok(result);
        }
    }
}
