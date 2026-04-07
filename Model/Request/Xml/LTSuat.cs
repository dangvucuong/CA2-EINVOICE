using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "LTSuat")]
    public class LTSuat
    {

        [XmlElement(ElementName = "TSuat")]
        public string ten_thue_suat { get; set; }

        [XmlElement(ElementName = "ThTien")]
        public string thanh_tien { get; set; }

        [XmlElement(ElementName = "TThue")]
        public string tien_thue { get; set; }
    }
}
