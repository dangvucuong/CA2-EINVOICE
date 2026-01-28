namespace Model.Request.HoaDon
{
    public class HoaDonPdfInforRequest
    {
        public string donvi_ma_dv { get; set; }
        public string ky_hieu { get; set; }
        public int fromMaSo { get; set; }
        public int toMaSo { get; set; }
    }
}