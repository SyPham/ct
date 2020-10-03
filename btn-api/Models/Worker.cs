using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace btn_api.Models
{
    public class Worker
    {
        public Worker()
        {
            this.CreatedDate = DateTime.Now;
        }
        [Key]
        public int ID { get; set; }
        public string FullName { get; set; }
        public string WorkerCode { get; set; }
        public string Operation { get; set; }
        public int? BuildingID { get; set; }
        public Building Building { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}