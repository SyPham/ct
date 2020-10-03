using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btn_api.DTO
{
    public class LineInfoDto
    {
        public int ID { get; set; }
        public string PO { get; set; }
        public string Batch { get; set; }
        public string ModelName { get; set; }
        public string ModelNO { get; set; }
        public string ArticleNO { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LineID { get; set; }
        public string LineName { get; set; }
    }
}
