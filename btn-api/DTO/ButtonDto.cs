using System.Collections.Generic;
using btn_api.Models;

namespace btn_api.DTO
{
    public class ButtonDto
    {

        public int ID { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int Standard { get; set; }
        public int TaktTime { get; set; }
        public int? WorkerID { get; set; }
        public Worker Worker { get; set; }
        public bool HasExist { get; set; }
        public bool Status { get; set; }
    }
}