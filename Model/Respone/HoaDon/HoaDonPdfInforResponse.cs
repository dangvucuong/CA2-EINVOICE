namespace Model.Respone.HoaDon
{
    public class HoaDonPdfInforResponse
    {
        public int id { get; set; }
        public int? ma_so_hoa_don { get; set; }
        public string hoa_don_dang_ky_phat_hanh_ky_hieu { get; set; }
        public string file_name { get; set; }
        public string html { get; set; }
    }
}