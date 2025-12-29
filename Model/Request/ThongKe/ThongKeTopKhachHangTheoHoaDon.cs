namespace Model.Request.ThongKe
{
    public class ThongKeTopKhachHangTheoHoaDonRequest
    {
        public string? donvi_ma_dv { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public int top { get; set; }
    }
}
