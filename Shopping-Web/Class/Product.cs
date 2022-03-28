using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping_Web.Class
{
    public class Product
    {
        public string id { get; set; }
        public bool enabled { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public string picture { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string createdDate { get; set; }
        public string updatedDate { get; set; }
    }
}