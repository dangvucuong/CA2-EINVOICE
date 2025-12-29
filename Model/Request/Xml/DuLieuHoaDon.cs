using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "DLHDon")]
    public class DuLieuHoaDon
    {

        [XmlElement(ElementName = "TTChung")]
        public ThongTinChung thong_tin_chung { get; set; }

        [XmlElement(ElementName = "NDHDon")]
        public NoiDungHoaDon noi_dung_hoa_don { get; set; }
        [XmlElement(ElementName = "TTKhac")]
        public ThongTinKhac thong_tin_khac { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string id { get; set; }

        // [XmlText]
        // public string Text { get; set; }
        public DuLieuHoaDon()
        {
            this.thong_tin_chung = new ThongTinChung();
            this.noi_dung_hoa_don = new NoiDungHoaDon();
        }
    }
}
