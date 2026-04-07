using Model.Base;

namespace Model.Table
{
    public class donvi_mua_chukyso : modify_infor
    {
        public int id { get; set; }
        public string donvi_mst { get; set; }
        public string nguoi_yeu_cau { get; set; }
        public string ma_goi_dich_vu { get; set; }
        public DateTime ngay_mua { get; set; }
        public int so_luong { get; set; }
        public int so_luong_khuyen_mai { get; set; }
        public int? tong_so_luong { get; set; }
        public string loai_san_pham { get; set; }
        public int trang_thai_hs_id { get; set; }
        public int trang_thai_tt_id { get; set; }
        public int trang_thai_ds_id { get; set; }
        public int tt_xuat_hd_id { get; set; }
        public string serial_number { get; set; }
        public string ten_cong_ty { get; set; }
        public string dia_chi { get; set; }
        public string transaction_id { get; set; }
        public int trang_thai_hd_id { get; set; }
    }
}