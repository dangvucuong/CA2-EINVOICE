using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TTin")]
    public class ThongTinKhacNoiDung
    {

        [XmlElement(ElementName = "TTruong")]
        public string thong_tin_truong { get; set; }

        [XmlElement(ElementName = "KDLieu")]
        public string kieu_du_lieu { get; set; }

        [XmlElement(ElementName = "DLieu")]
        public string du_lieu { get; set; }
    }
}
