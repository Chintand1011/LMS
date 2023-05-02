using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Entities.View.Shared
{
    public class UserInfoVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }//Company Name
        public string DeptCode { get; set; }
       
        public string ImageUrl { get; set; }
        public string username { get; set; }



    }
}