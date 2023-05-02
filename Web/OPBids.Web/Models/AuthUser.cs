using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace OPBids.Web.Models
{
    public class AuthUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string GroupId { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string department_code { get; set; }
        public string department_id { get; set; }
        public string department_name { get; set; }
        public int dashboard_id { get; set; }
        public bool pmfs_access { get; set; }
        public bool dts_access { get; set; }
        public bool vip { get; set; }
    }
}