using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitygateApi.Models
{
    public class Analytics
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Sessions { get; set; }
        public int PageViews { get; set; }
    }
}