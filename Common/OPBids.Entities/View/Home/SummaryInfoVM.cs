using System.Collections.Generic;
using System.Xml.Serialization;
using OPBids.Entities.Base;

namespace OPBids.Entities.View.Home
{
    [XmlRoot(ElementName = "SummaryInfoList")]
    public class SummaryInfoList {
        [XmlElement("SummaryInfo")]
        public List<SummaryInfo> SummaryInfos { get; set; }
    }

    public class SummaryInfo
    {
        [XmlElement("title")]
        public string title { get; set; }

        [XmlElement("type")]
        public int type { get; set; }

        [XmlElement("iconCls")]
        public string iconCls { get; set; }

        [XmlElement("items")]
        public List<SummaryItem> items{ get; set; }
    }
}