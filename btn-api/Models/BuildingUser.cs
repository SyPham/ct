﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.Models
{
    public class BuildingUser
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int BuildingID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
