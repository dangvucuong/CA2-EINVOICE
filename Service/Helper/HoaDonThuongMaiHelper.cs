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
    }
}
