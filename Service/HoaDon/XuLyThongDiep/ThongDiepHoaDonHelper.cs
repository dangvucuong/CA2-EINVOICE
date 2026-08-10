using System.Text.RegularExpressions;
using Model.Respone.Xml;

namespace Service.HoaDon.XuLyThongDiep
{
    public static class ThongDiepHoaDonHelper
    {
        public static string GetMLTDiep(KetQuaThongDiepRespone thongDiepRespone, string xmlKetQua)
        {
            var mltdiep = thongDiepRespone?.TTChung?.MLTDiep?.Trim() ?? "";
            if (!string.IsNullOrEmpty(mltdiep))
            {
                return mltdiep;
            }

            return ExtractXmlTagValue(xmlKetQua, "MLTDiep");
        }

        public static bool IsLoiThongDiep(KetQuaThongDiepRespone thongDiepRespone, string xmlKetQua)
        {
            if (string.IsNullOrWhiteSpace(xmlKetQua))
            {
                return GetMLTDiep(thongDiepRespone, xmlKetQua) == "-1";
            }

            if (xmlKetQua.Contains("<MLTDiep>-1</MLTDiep>"))
            {
                return true;
            }

            var mltdiep = GetMLTDiep(thongDiepRespone, xmlKetQua);
            if (mltdiep == "-1")
            {
                return true;
            }

            if (mltdiep == "999" && xmlKetQua.Contains("<TTTNhan>1</TTTNhan>"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Thông điệp 999 thuần chỉ lưu log; lỗi -1 (kể cả lồng trong 999) phải chạy đầy đủ XuLyThongDiep.
        /// </summary>
        public static bool ShouldRunFullXuLy(KetQuaThongDiepRespone thongDiepRespone, string xmlKetQua)
        {
            if (IsLoiThongDiep(thongDiepRespone, xmlKetQua))
            {
                return true;
            }

            return GetMLTDiep(thongDiepRespone, xmlKetQua) != "999";
        }

        public static string ExtractXmlTagValue(string xml, string tagName)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return "";
            }

            var match = Regex.Match(xml, $@"<{tagName}>(.*?)</{tagName}>", RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : "";
        }
    }
}
