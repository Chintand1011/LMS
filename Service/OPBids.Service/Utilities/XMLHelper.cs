using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace OPBids.Service.Utilities
{
    public class XMLHelper<T>
    {
        public T Deserialize(string path)
        {
            string xmlInputData = File.ReadAllText(HttpContext.Current.Server.MapPath(path));
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xmlInputData))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}