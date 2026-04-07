using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "NDHDon")]
    public class NoiDungHoaDon
    {

        [XmlElement(ElementName = "NBan")]
        public NguoiBan nguoi_ban { get; set; }

        [XmlElement(ElementName = "NMua")]
        public NguoiMua nguoi_mua { get; set; }

        [XmlElement(ElementName = "DSHHDVu")]
        public DanhSachHangHoaDichVu danh_sach_hang_hoa_dich_vu { get; set; }

        [XmlElement(ElementName = "TToan")]
        public ThongTinThanhToan thong_tin_thanh_toan { get; set; }
    }
}
