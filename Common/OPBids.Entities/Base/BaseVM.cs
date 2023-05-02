using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.Base
{
    public class BaseVM
    {
        public virtual int id { get; set; }
        public int created_by { get; set; }
        public int page_index { get; set; }
        public string created_by_name { get; set; }
        public string created_date { get; set; }
        public string created_time { get; set; }
        public int updated_by { get; set; }
        public string updated_by_name { get; set; }
        public string updated_date { get; set; }
        public string search_key { get; set; }
        public string[] item_list { get; set; }
        public DateTime sort_date { get; set; }
    }
}
