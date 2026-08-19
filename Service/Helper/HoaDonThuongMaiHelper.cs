using System;

namespace Service.Helper
{
    public static class HoaDonThuongMaiHelper
    {
        public static bool IsHoaDonThuongMai(string mauSo, string kyHieu)
        {
            if (!string.Equals((mauSo ?? "").Trim(), "7", StringComparison.Ordinal))
                return false;
            if (string.IsNullOrWhiteSpace(kyHieu) || kyHieu.Length < 4)
                return false;
            return kyHieu.Substring(3, 1).Equals("X", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Hóa đơn thương mại + VND: luôn gen TGia = 1. Hóa đơn thường: VND không gen TGia.
        /// </summary>
        public static string ResolveTyGiaXml(string mauSo, string kyHieu, string loaiTien, string tyGiaKhongVnd)
        {
            var dvt = (loaiTien ?? "").Trim();
            if (IsHoaDonThuongMai(mauSo, kyHieu) && string.Equals(dvt, "VND", StringComparison.OrdinalIgnoreCase))
            {
                return "1";
            }
            if (!string.Equals(dvt, "VND", StringComparison.OrdinalIgnoreCase) && dvt != "")
            {
                return tyGiaKhongVnd;
            }
            return null;
        }
    }
}
