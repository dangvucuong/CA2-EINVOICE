using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "TTHDLQuan")]
    public class hd_thong_tin_bo_sung
    {
        public int is_hd_phi_thue_quan { get; set; }
        public string so_quyet_dinh { get; set; }
        public string? ngay_quyet_dinh { get; set; }
        public string co_quan_ban_hanh_qd { get; set; }
        public string hinh_thuc_ban { get; set; }
        public string dia_diem_vc_hang_den { get; set; }
        public string? tgian_vc_hang_den_tu { get; set; }
        public string? tgian_vc_hang_den_den { get; set; }
    }

}
