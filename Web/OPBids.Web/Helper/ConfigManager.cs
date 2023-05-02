using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OPBids.Web.Helper
{
    public static class ConfigManager
    {
        public static string BaseServiceURL
        {
            get { return ConfigurationManager.AppSettings["BaseServiceURL"]; }
        }
        public static string AesKey
        {
            get { return ConfigurationManager.AppSettings["AesKey"]; }
        }
        public static string AesIV
        {
            get { return ConfigurationManager.AppSettings["AesIV"]; }
        }

    }
}