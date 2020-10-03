using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.Models
{
    public class UserButton
    {
        public UserButton()
        {
            CreatedDate = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ButtonID { get; set; }
        public Building Building { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
