using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPBids.Web.Models
{
    public class UserMessageModel
    {
        public string Id { get; set; }
        public string Sender { get; set; }
        public string Time { get; set; }
        public string Preview { get; set; }
    }
}