using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TToan")]
    public class ThongTinThanhToan
    {

        [XmlElement(ElementName = "THTTLTSuat")]
        public THTTLTSuat thong_tin_thue_suat { get; set; }


        [XmlElement(ElementName = "TgTCThue")]
        public string tong_tien_chua_thue { get; set; }

        [XmlElement(ElementName = "TgTThue")]
        public string tong_tien_thue { get; set; }

        [XmlElement(ElementName = "TTCKTMai")]
        public string tong_tien_chiet_khau { get; set; }


        [XmlElement(ElementName = "TgTTTBSo")]
        public string tong_tien_thanh_toan_bang_so { get; set; }

        [XmlElement(ElementName = "TgTTTBChu")]
        public string tong_tien_thanh_toan_bang_chu { get; set; }
        [XmlElement(ElementName = "DSLPhi")]
        public DSLPhi thong_tin_phis { get; set; }
    }
}
