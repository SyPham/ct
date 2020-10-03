using AutoMapper;
using AutoMapper.QueryableExtensions;
using btn_api._Repositories.Interface;
using btn_api._Services.Interface;
using btn_api.DTO;
using btn_api.Helpers;
using btn_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Services.Services
{
    public class BuildingWorkerService : IBuildingWorkerService
    {
        private readonly IBuildingWorkerRepository _buildingUserRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public BuildingWorkerService(IBuildingWorkerRepository buildingUserRepository,
            IBuildingRepository buildingRepository,
            IMapper mapper,
            MapperConfiguration configMapper)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _buildingUserRepository = buildingUserRepository;
            _buildingRepository = buildingRepository;
        }

        public Task<bool> Add(BuildingWorkerDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BuildingWorkerDto>> GetAllAsync()
        {
            return await _buildingUserRepository.FindAll().ProjectTo<BuildingWorkerDto>(_configMapper).ToListAsync();

        }

        public async Task<object> GetBuildingByWorkerID(int workerId)
        {
            var model =await _buildingUserRepository.FindAll().FirstOrDefaultAsync(x => x.WorkerID == workerId);
            return _buildingRepository.FindById(model.BuildingID);
        }

        public async Task<List<BuildingWorkerDto>> GetBuildingWorkerByBuildingID(int buildingID)
        {
            return await _buildingUserRepository.FindAll().Where(x=>x.BuildingID == buildingID).ProjectTo<BuildingWorkerDto>(_configMapper).ToListAsync();
        }

        public BuildingWorkerDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<BuildingWorkerDto>> GetWithPaginations(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public async Task<object> MapBuildingWorker(int workerId, int buildingid)
        {
            var item = await _buildingUserRepository.FindAll().FirstOrDefaultAsync(x => x.WorkerID == workerId && x.BuildingID == buildingid);
            if (item == null)
            {
                _buildingUserRepository.Add(new BuildingWorker
                {
                    WorkerID = workerId,
                    BuildingID = buildingid,
                });
                try
                {
                    await _buildingUserRepository.SaveAll();
                    return new
                    {
                        status = true,
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            }
            else
            {
                item.WorkerID = workerId;
                item.BuildingID = buildingid;

                try
                {
                    await _buildingUserRepository.SaveAll();
                    return new
                    {
                        status = true,
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            }
        }

        public async Task<object> MappingUserWithBuilding(BuildingWorkerDto buildingUserDto)
        {
            var item =await _buildingUserRepository.FindAll().FirstOrDefaultAsync(x => x.WorkerID == buildingUserDto.WorkerID && x.BuildingID == buildingUserDto.BuildingID);
            if (item == null)
            {
                _buildingUserRepository.Add(new BuildingWorker { 
                    WorkerID = buildingUserDto.WorkerID,
                    BuildingID = buildingUserDto.BuildingID
                });
                try
                {
                   await _buildingUserRepository.SaveAll();
                    return new
                    {
                        status = true,
                        message = "Mapping Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on save!"
                    };
                }
            } else
            {

                return new
                {
                    status = false,
                    message = "The User belonged with other building!"
                };
            }
        }

        public async Task<object> RemoveBuildingWorker(BuildingWorkerDto buildingUserDto)
        {
            var item = await _buildingUserRepository.FindAll().FirstOrDefaultAsync(x => x.WorkerID == buildingUserDto.WorkerID && x.BuildingID == buildingUserDto.BuildingID);
            if (item != null)
            {
                _buildingUserRepository.Remove(item);
                try
                {
                    await _buildingUserRepository.SaveAll();
                    return new
                    {
                        status = true,
                        message = "Delete Successfully!"
                    };
                }
                catch (Exception)
                {
                    return new
                    {
                        status = false,
                        message = "Failed on delete!"
                    };
                }
            }
            else
            {

                return new
                {
                    status = false,
                    message = ""
                };
            }
        }

        public Task<PagedList<BuildingWorkerDto>> Search(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BuildingWorkerDto model)
        {
            throw new NotImplementedException();
        }
    }
}
