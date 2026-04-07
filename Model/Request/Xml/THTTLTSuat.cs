using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "THTTLTSuat")]
    public class THTTLTSuat
    {

        [XmlElement(ElementName = "LTSuat")]
        public List<LTSuat> thue_suats { get; set; }
    }

}
