using Model.Base;

namespace Model.Table
{
    public class bang_tong_hop_du_lieu_log : modify_infor
    {
        public int id { get; set; }
        public int bang_tong_hop_du_lieu_id { get; set; }
        public int bang_tong_hop_du_lieu_log_type_id { get; set; }
        public string nguoi_thuc_hien { get; set; }
        public DateTime ngay_thuc_hien { get; set; }
        public string noi_dung_thuc_hien { get; set; }
        public string file_thong_diep_url { get; set; }
    }
}