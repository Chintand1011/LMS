using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OPBids.Service.Models.Base;
using static OPBids.Common.Enum;

namespace OPBids.Service.Models.Home
{
    public class SummaryInfo
    {
        public string title { get; set; }
        public SurveyType type { get; set; }
        public List<SummaryItem> items { get; set; }
    }
}