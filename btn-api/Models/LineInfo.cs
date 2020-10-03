using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace btn_api.Models
{
    public class LineInfo
    {
        public LineInfo()
        {
            this.CreatedDate = DateTime.Now;
        }
        [Key]
        public int ID { get; set; }
        public string PO { get; set; }
        public string Batch { get; set; }
        public string ModelName { get; set; }
        public string ModelNO { get; set; }
        public string ArticleNO { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LineID { get; set; }
        [ForeignKey("LineID")]
        public Building Building { get; set; }

    }
}