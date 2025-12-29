using Model.Base;

namespace Model.Table
{
    public class thong_bao_sai_sot_chi_tiet : modify_infor
    {
        public int id { get; set; }
        public int thong_bao_sai_sot_id { get; set; }
        public int hoa_don_id { get; set; }
        public string hoa_don_dang_ky_phat_hanh_mau_so { get; set; }
        public string hoa_don_dang_ky_phat_hanh_ky_hieu { get; set; }
        public string ma_so_hoa_don { get; set; }
        public DateTime ngay_hoa_don { get; set; }
        public string ma_cqt_cap { get; set; }
        
        
    }
}