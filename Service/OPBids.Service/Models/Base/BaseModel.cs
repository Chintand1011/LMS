using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static OPBids.Common.Enum;
namespace OPBids.Service.Models.Base
{
    public class BaseModel
    {
        public int id { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int updated_by { get; set; }
        public DateTime updated_date { get; set; } = DateTime.Now;

    }
}