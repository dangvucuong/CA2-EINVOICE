using Model.Base;

namespace Model.Table
{
    public class thong_bao_sai_sot : modify_infor
    {
        public int id { get; set; }
        public string donvi_ma_dv { get; set; }
        public string phien_ban { get; set; }
        public string ma_so { get; set; }
        public string ten_thong_bao { get; set; }
        public string ma_cqt { get; set; }
        public string ten_cqt { get; set; }
        public string so_thong_bao { get; set; }
        public string ten_nguoi_nop_thue { get; set; }
        public string dia_danh { get; set; }
        public DateTime ngay_thong_bao { get; set; }
        public int thong_bao_sai_sot_trang_thai_id { get; set; }
        public int thong_bao_sai_sot_tinh_chat_id { get; set; }
        public int loai_hoa_don_dien_tu_id { get; set; }
        public string? ket_qua_phan_hoi { get; set; }
        public string ly_do { get; set; }
        public string? phat_hanh_uuid { get; set; }
        public int? user_id_phathanh { get; set; }
        
        
    }
}