namespace Model.Request.HoaDon
{
    public class HoaDonThongTinBoSung
    {
        public int IsHdBanTaiSanCong { get; set; }
        public string SoQuyetDinh { get; set; }
        public DateTime? NgayQuyetDinh { get; set; }
        public string CoQuanBanHanhQD { get; set; }
        public string HinhThucBan { get; set; }
        public string DiaDiemVCHangDen { get; set; }
        public DateTime? TgianVCHangDenTu { get; set; }
        public DateTime? TgianVCHangDenDen { get; set; }
        public int IsHdPhiThueQuan { get; set; }
    }
}