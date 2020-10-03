using AutoMapper;
using btn_api._Repositories.Interface;
using btn_api.Data;
using btn_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api._Repositories.Repositories
{
    public class BuildingWorkerRepository : ECRepository<BuildingWorker>, IBuildingWorkerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BuildingWorkerRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

    }
}
