using Model.Base;

namespace Model.Table
{
    public class donvi : modify_infor
    {
        public int id { get; set; }
        public string ma_dv { get; set; }
        public string ten_dv { get; set; }
        public string dia_chi { get; set; }
        public string? mst { get; set; }
        public string stk { get; set; }
        public string dien_thoai { get; set; }
        public string? ghi_chu { get; set; }
        public string donvi_chuquan { get; set; }
        public string ngan_hang { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string email { get; set; }

        //dùng để tạo mã mtt
        public string? ma_dang_ky_cqt { get; set; }

        public int co_quan_thu_id_chuquan { get; set; }
        public DateTime? ngay_hoa_don_max { get; set; }
        public int to_khai_success_id { get; set; }
        public string serials { get; set; }
        public int? total_cks_con_lai { get; set; }



    }
}