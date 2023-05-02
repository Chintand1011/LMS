using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Entities.View.Shared
{
    public class UserNavVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int RoleId{ get; set; }
        public string IconCls { get; set; }
    }
}