using Model.Base;

namespace Model.Table
{
    public class dm_hanghoa : modify_infor
    {
        public int id { get; set; }
        public string donvi_ma_dv { get; set; }
        public string ma_hang_hoa { get; set; }
        public string ten_hang_hoa { get; set; }
        public string dvt { get; set; }
        public string ma_loai_hoang_hoa { get; set; }
        public decimal? don_gia { get; set; }
    }
}