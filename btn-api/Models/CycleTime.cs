using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.Models
{
    [Table("cycletime1")]
    public class CycleTime
    {
        [Key]
        public int id { get; set; }
        public int BtnID { get; set; }
        public float cycleTime { get; set; }
        public DateTime datetime { get; set; }
    }
}
