using System.Threading.Tasks;
using btn_api._Repositories.Interface;
using btn_api.Data;
using btn_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using btn_api.DTO;
using System.Collections.Generic;

namespace btn_api._Repositories.Repositories
{
    public class LineInfoRepository : ECRepository<LineInfo>, ILineInfoRepository
    {
        private readonly DataContext _context;
        public LineInfoRepository(DataContext context) : base(context)
        {
            _context = context;
        }

     
        //Login khi them repo
    }
}