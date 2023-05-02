using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Service.Models.Home
{
    public class SummaryItem
    {
        public string label { get; set; }
        public decimal value { get; set; }
        public int count { get; set; }
        public string color { get; set; }
        
    }
}