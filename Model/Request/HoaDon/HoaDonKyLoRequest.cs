namespace Model.Request.HoaDon
{
    public class HoaDonKyLoRequest
    {
        public string progress_id { get; set; }
        public List<int> ids { get; set; }
        public string? rs_ma_but_ky { get; set; }

    }
}