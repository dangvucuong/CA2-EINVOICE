using Model.Respone.Xml;

namespace Model.Respone.HoaDon
{
    public class HoaDonPhatHanhRespone
    {
        public int id { get; set; }
        public int hoa_don_trang_thai_id { get; set; }
        public string ket_qua_phat_hanh { get; set; }
        public string file_thong_diep_url { get; set; }
        
        
        public KetQuaThongDiepRespone KetQuaThongDiep { get; set; }
    }
}
