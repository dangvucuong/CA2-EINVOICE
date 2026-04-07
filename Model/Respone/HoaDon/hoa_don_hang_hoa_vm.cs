using Model.Table;

namespace Model.Respone.HoaDon
{
    public class hoa_don_hang_hoa_vm : hoa_don_hang_hoa
    {
        public string loai_hoa_don_ct_name { get; set; }
        
        public string hoa_don_dang_ky_phat_hanh_mau_so { get; set; }
        public string hoa_don_dang_ky_phat_hanh_ky_hieu { get; set; }
        public string? ma_so_hoa_don { get; set; }
        public DateTime ngay_hoa_don { get; set; }
        public string nguoi_mua_mst { get; set; }
        public string nguoi_mua_ten_donvi { get; set; }
        public string nguoi_mua_ten { get; set; }
        public string nguoi_mua_dia_chi { get; set; }
        public int hoa_don_hinh_thuc_id { get; set; }
        public string ma_tra_cuu { get; set; }
        public string ma_dv_ngan_sach { get; set; }
        public string nguoi_mua_cccd { get; set; }
        public int hoa_don_trang_thai_id { get; set; }
        public string? hoa_don_dang_ky_phat_hanh_mau_so_goc { get; set; }
        public string? hoa_don_dang_ky_phat_hanh_ky_hieu_goc { get; set; }
        public string? ma_so_hoa_don_goc { get; set; }

    }
}