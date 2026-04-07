using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "DSLPhi")]
    public class DSLPhi
    {
        [XmlElement(ElementName = "LPhi")]
        public List<LPhi> loai_phis { get; set; }
    }
    [XmlRoot(ElementName = "LPhi")]
    public class LPhi
    {
        [XmlElement(ElementName = "TLPhi")]
        public string ten_loai_phi { get; set; }

        [XmlElement(ElementName = "TPhi")]
        public string tien_phi { get; set; }
    }
}