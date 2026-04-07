using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TTKhac")]
    public class ThongTinKhac
    {

        [XmlElement(ElementName = "TTin")]
        public List<ThongTinKhacNoiDung> thong_tin_khac_noi_dung { get; set; }
    }
  
}
