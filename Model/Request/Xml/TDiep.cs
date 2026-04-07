using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TDiep")]
    public class ThongDiep
    {

        [XmlElement(ElementName = "TTChung")]
        public ThongTinChungThongDiep ThongTinChung { get; set; }

        [XmlElement(ElementName = "DLieu")]
        public DLieu DuLieu { get; set; }
    }
}