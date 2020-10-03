using System.Threading.Tasks;
using btn_api._Repositories.Interface;
using btn_api.Data;
using btn_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using btn_api.DTO;
using System.Collections.Generic;
using AutoMapper;

namespace btn_api._Repositories.Repositories
{
    public class CycleTimeRepository : IoTRepository<CycleTime>, ICycleTimeRepository
    {
        private readonly IoTContext _context;
        public CycleTimeRepository(IoTContext context) : base(context)
        {
            _context = context;
        }
    }
}