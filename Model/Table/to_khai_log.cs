using Model.Base;

namespace Model.Table
{
    public class to_khai_log : modify_infor
    {
        public int id { get; set; }
        public int to_khai_id { get; set; }
        public int to_khai_log_type_id { get; set; }
        public string nguoi_thuc_hien { get; set; }
        public DateTime ngay_thuc_hien { get; set; }
        public string noi_dung_thuc_hien { get; set; }
        public string file_thong_diep_url { get; set; }
    }
}