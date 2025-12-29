using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "DSHHDVu")]
    public class DanhSachHangHoaDichVu
    {

        [XmlElement(ElementName = "HHDVu")]
        public List<HangHoaDichVu> hang_hoa_dich_vus { get; set; }
        public DanhSachHangHoaDichVu()
        {
            this.hang_hoa_dich_vus = new List<HangHoaDichVu>();
        }
    }
}
