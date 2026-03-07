namespace Model.Request.HoaDon
{
    public class HoaDonThongTinBoSung
    {
        public int IsHdBanTaiSanCong { get; set; }
        public string SoQuyetDinh { get; set; }
        public string? NgayQuyetDinh { get; set; }
        public string CoQuanBanHanhQD { get; set; }
        public string HinhThucBan { get; set; }
        public string DiaDiemVCHangDen { get; set; }
        public string? TgianVCHangDenTu { get; set; }
        public string? TgianVCHangDenDen { get; set; }
        public int IsHdPhiThueQuan { get; set; }
    }
}