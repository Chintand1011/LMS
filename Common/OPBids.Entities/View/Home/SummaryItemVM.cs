using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OPBids.Entities.View.Home
{
    public class SummaryItem
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("label")]
        public string label { get; set; }

        public decimal value { get; set; }

        public int count { get; set; }

        [XmlAttribute("color")]
        public string color { get; set; }

        [XmlAttribute("menuId")]
        public string menuId { get; set; }
        [XmlAttribute("subMenuId")]
        public string subMenuId { get; set; }

    }
}