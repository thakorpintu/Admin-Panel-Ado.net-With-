using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce1.Models
{
    public class Category_Model
    {
        public int  Cid { get; set; }
        public string Category { get; set; }
        public byte Status { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}