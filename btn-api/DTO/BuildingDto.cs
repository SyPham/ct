using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.DTO
{
    public class BuildingDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentID { get; set; }
    }
    
}
