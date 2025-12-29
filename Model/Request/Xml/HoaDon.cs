using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "HDon")]
    public class HoaDon
    {

        [XmlElement(ElementName = "DLHDon")]
        public DuLieuHoaDon du_lieu_hoa_don { get; set; }

        /// <summary>
        /// Chi gan gia tri khi la HD May tinh tien (Ky tu thu 4 = M)
        /// </summary>
        [XmlElement(ElementName = "MCCQT")]
        public string ma_co_quan_thue { get; set; }

        [XmlElement(ElementName = "DLQRCode")]
        public string qr_code { get; set; }
        

        [XmlElement(ElementName = "DSCKS")]
        public DanhSachChuKySo danh_sach_chu_ky_so { get; set; }

        public HoaDon()
        {
            this.du_lieu_hoa_don = new DuLieuHoaDon();
            this.danh_sach_chu_ky_so = new DanhSachChuKySo();
        }
    }
}
