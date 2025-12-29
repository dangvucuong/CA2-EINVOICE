using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "DSCKS")]
    public class DanhSachChuKySo
    {

        [XmlElement(ElementName = "NBan")]
        public CKSNguoiBan nguoi_ban { get; set; }

        [XmlElement(ElementName = "NMua")]
        public CKSNguoiMua nguoi_mua { get; set; }
    }
}
