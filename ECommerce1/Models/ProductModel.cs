using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce1.Models
{
    public class ProductModel
    {
        public int Pid { get; set; }
        public int Cid { get; set; }
        public int Sid { get; set; }
        public int Tid { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductColor { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public HttpPostedFileBase fileupload { get; set; }
        public DateTime EnterDate { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ThirdCategory { get; set; }
        

    }
}