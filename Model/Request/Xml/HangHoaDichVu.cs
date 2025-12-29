using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "HHDVu")]
    public class HangHoaDichVu
    {

        [XmlElement(ElementName = "TChat")]
        public int tinh_chat { get; set; }

        [XmlElement(ElementName = "STT")]
        public string stt { get; set; }

        [XmlElement(ElementName = "MHHDVu")]
        public string ma_hang_hoa_dich_vu { get; set; }

        [XmlElement(ElementName = "THHDVu")]
        public string ten_hang_hoa_dich_vu { get; set; }

        [XmlElement(ElementName = "DVTinh")]
        public string don_vi_tinh { get; set; }

        [XmlElement(ElementName = "SLuong")]
        public string so_luong { get; set; }

        [XmlElement(ElementName = "DGia")]
        public string don_gia { get; set; }

        [XmlElement(ElementName = "TLCKhau")]
        public string ty_le_chiet_khau { get; set; }
        [XmlElement(ElementName = "STCKhau")]
        public string so_tien_chiet_khau { get; set; }

        [XmlElement(ElementName = "ThTien")]
        public string thanh_tien { get; set; }

        [XmlElement(ElementName = "TSuat")]
        public string thue_suat { get; set; }
        [XmlElement(ElementName = "TTHHDTrung")]
        public TTHHDTrung TTHHDTrung { get; set; }
    }
    public class TTHHDTrung
    {
        [XmlElement(ElementName = "TTin")]
        public List<TTHHDTrungTTin> TTHHDTrungTTins { get; set; }
    }
    [XmlRoot(ElementName = "TTin")]
    public class TTHHDTrungTTin
    {
        public string LHHDTrung { get; set; }
        public string TTruong { get; set; }
        public string DLieu { get; set; }
    }
    public class TTHHDTrungInfo
    {
        public string LHHDTrung { get; set; }
        public string SKhung { get; set; }
        public string SMay { get; set; }
        public string BKSPTVChuyen { get; set; }
        public string TNGHang { get; set; }
        public string DCNGHang { get; set; }
        public string MSTNGHang { get; set; }
        public string MDDNGHang { get; set; }
    }

}
