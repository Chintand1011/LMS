using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.Common
{
    public class Payload : Base.BaseVM
    {
        public int user_id { get; set; }
        public int group_id { get; set; }
        public string search_key { get; set; }
        public string[] item_list { get; set; }
        public string status { get; set; }
        public string auth_x_un { get; set; }
        public string auth_x_pwd { get; set; }
        public string auth_x_code { get; set; }

        public string[] setting_list { get; set; }
        public int dashboard_id { get; set; }
    }
}
