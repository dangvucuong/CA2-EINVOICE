using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Service.Helper
{
    public class TienThueTheoMuc
    {
        public TienThueTheoMuc(decimal thanhTienTinhThue, decimal phanTram)
        {
            ThanhTienTinhThue = thanhTienTinhThue;
            PhanTram = phanTram;
        }

        public decimal ThanhTienTinhThue { get; }
        public decimal PhanTram { get; }
    }

    public static class HoaDonTienThueHelper
    {
        public static bool IsVnd(string loaiTien)
        {
            var dvt = loaiTien.ConvertToString().Trim().ToUpper();
            return dvt == "" || dvt == "VND";
        }

        public static decimal GetPhanTramThue(string thueSuat)
        {
            return (decimal)thueSuat.ConvertToString().Replace("KHAC:", "").Replace("%", "").Trim().ConvertToDouble(2);
        }

        /// <summary>
        /// Tiền thuế VND theo từng mức: tổng tiền thuế chỉ làm tròn một lần từ tổng chưa làm tròn,
        /// phần chênh do làm tròn chia cho các mức có phần lẻ lớn nhất (bằng nhau thì ưu tiên thuế suất cao, rồi tiền tính thuế lớn),
        /// nên tổng tiền thuế các mức luôn bằng Round(tổng tiền thuế chưa làm tròn).
        /// Phải giữ giống hệt phanBoTienThueVnd ở ClientApp/src/helpers/tienThueHelper.ts.
        /// </summary>
        public static List<decimal> PhanBoTienThueVnd(IList<TienThueTheoMuc> items)
        {
            var exacts = items.Select(x => x.ThanhTienTinhThue * x.PhanTram / 100m).ToList();
            var sign = exacts.All(x => x <= 0) ? -1m : 1m;
            var values = exacts.Select(x => x * sign).ToList();

            var target = Math.Round(values.Sum(), 0, MidpointRounding.AwayFromZero);
            var result = values.Select(Math.Floor).ToList();
            var k = (int)(target - result.Sum());

            var order = Enumerable.Range(0, values.Count)
                .OrderByDescending(i => values[i] - result[i])
                .ThenByDescending(i => items[i].PhanTram)
                .ThenByDescending(i => Math.Abs(items[i].ThanhTienTinhThue))
                .ThenBy(i => i)
                .ToList();
            for (var j = 0; j < k && j < order.Count; j++)
            {
                result[order[j]] += 1;
            }

            return result.Select(x => x * sign).ToList();
        }
    }
}
