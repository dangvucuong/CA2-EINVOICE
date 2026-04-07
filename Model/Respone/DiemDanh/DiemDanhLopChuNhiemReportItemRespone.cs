namespace Model.Respone.DiemDanh
{
   public class DiemDanhLopChuNhiemReportItemRespone
    {
        public string id { get; set; }
        public int dm_truong_id { get; set; }
        public string ten_truong { get; set; }
        public string ten_truong_en { get; set; }
        public int dm_khoi_id { get; set; }
        public string ten_khoi { get; set; }
        public string ten_khoi_en { get; set; }
        public int dm_he_id { get; set; }
        public string ten_he { get; set; }
        public string ten_he_en { get; set; }
        public int dm_lop_id { get; set; }
        public string ten_lop { get; set; }
        public string ten_lop_en { get; set; }
        public int sis_diemdanh_status_id { get; set; }
        public string sis_diemdanh_status_name { get; set; }
        public string sis_diemdanh_status_name_en { get; set; }
        public int so_luong { get; set; }
        public int si_so { get; set; }


    }
}