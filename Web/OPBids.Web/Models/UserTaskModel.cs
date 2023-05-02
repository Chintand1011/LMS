using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Web.Models
{
    public class UserTaskModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int Completion { get; set; }
    }
}