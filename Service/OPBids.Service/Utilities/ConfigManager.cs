using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OPBids.Service.Utilities
{
    public static class ConfigManager
    {
        public static string DashboardRecord
        {
            get { return ConfigurationManager.AppSettings["DashboardRecord"]; }
        }
    }
}