using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "DLieu")]
    public class DLieu
    {

        [XmlElement(ElementName = "HDon")]
        public HoaDon HoaDon { get; set; }
        
        [XmlAttribute(AttributeName = "Id")]
        public string? id { get; set; }
    }
}