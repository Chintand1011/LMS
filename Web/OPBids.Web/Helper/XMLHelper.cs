using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace OPBids.Web.Helper
{
    public class XMLHelper <T>
    {
        public T Deserialize(string path) {
            //~/App_Data/TernTenders.xml
            //XmlTextReader xmlreader = new XmlTextReader(HttpContext.Current.Server.MapPath(path));                       
            string xmlInputData = File.ReadAllText(HttpContext.Current.Server.MapPath(path));
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xmlInputData))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}