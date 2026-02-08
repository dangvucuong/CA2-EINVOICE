using Model.Base;

namespace Model.Table
{
    public class khachhang : modify_infor
    {
        public int id { get; set; }
        public string donvi_ma_dv { get; set; }
        public string ten_khach_hang { get; set; }
        public string ten_don_vi { get; set; }
        public string dia_chi { get; set; }
        public string? stk { get; set; }
        public string mst { get; set; }
        public string email { get; set; }
        public string ma_dv_ngan_sach { get; set; }

        public string? ccdan { get; set; }

    }
}