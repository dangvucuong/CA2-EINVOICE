using Model.Base;

namespace Model.Table
{
    public class dai_ly : modify_infor
    {
        public int id { get; set; }
        public string donvi_ma_dv { get; set; }
        public string ma_dai_ly { get; set; }
        public string ten_dai_ly { get; set; }
        public string email { get; set; }
        public string so_tai_khoan { get; set; }
    }
}