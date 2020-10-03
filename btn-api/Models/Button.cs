using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.Models
{
    public class Button
    {
        [Key]
        public int ID { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Standard { get; set; }
        public int TaktTime { get; set; }
        public int? WorkerID { get; set; }
        public Worker Worker { get; set; }

    }
}
