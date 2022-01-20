using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce1.Models
{
    public class ThirdCategoryModel
    {
        public int Tid { get; set; }
        public int Sid { get; set; }
        public string Cid { get; set; }
        public string SubCategory { get; set; }
        public string CategoryName { get; set; }
        public string ThirdCategory { get; set; }

        public byte Status { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}