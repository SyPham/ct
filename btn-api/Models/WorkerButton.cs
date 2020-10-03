using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.Models
{
    public class WorkerButton
    {
        [Key, Column(Order = 1)]
        public int WorkrerID { get; set; }
        public Worker Worker { get; set; }
        [Key, Column(Order = 2)]
        public int ButtonID { get; set; }
        public Button Button { get; set; }

    }
}
