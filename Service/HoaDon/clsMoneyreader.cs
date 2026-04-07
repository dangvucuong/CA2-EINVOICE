using System;

namespace Service.HoaDon
{
    public static class clsMoneyreader
    {
        #region cấu hình đọc số

        private static readonly string[] ChuSo =
        {
            "không","một","hai","ba","bốn","năm","sáu","bảy","tám","chín"
        };

        private static readonly string[] Hang =
        {
            "","nghìn","triệu","tỷ"
        };

        #endregion

        #region cấu hình tiền tệ

        public class CurrencyInfo
        {
            public string MajorName { get; set; }

            public string MinorName { get; set; }

            public int MinorDigits { get; set; }

            public bool UseAnd { get; set; } = true;
        }

        static readonly Dictionary<string, CurrencyInfo> CurrencyMap =
            new Dictionary<string, CurrencyInfo>(StringComparer.OrdinalIgnoreCase)
            {
                ["VND"] = new CurrencyInfo
                {
                    MajorName = "đồng",
                    MinorDigits = 0,
                    UseAnd = false
                },

                ["USD"] = new CurrencyInfo
                {
                    MajorName = "đô la Mỹ",
                    MinorName = "cent",
                    MinorDigits = 2,
                    UseAnd = true
                },

                ["EUR"] = new CurrencyInfo
                {
                    MajorName = "euro",
                    MinorName = "cent",
                    MinorDigits = 2,
                    UseAnd = true
                },

                ["SGD"] = new CurrencyInfo
                {
                    MajorName = "đô la Singapore",
                    MinorName = "cent",
                    MinorDigits = 2,
                    UseAnd = true
                },

                ["JPY"] = new CurrencyInfo
                {
                    MajorName = "yên",
                    MinorName = "xu",
                    MinorDigits = 2,
                    UseAnd = false
                },

                ["CHF"] = new CurrencyInfo
                {
                    MajorName = "franc Thụy Sĩ",
                    MinorName = "centime",
                    MinorDigits = 2,
                    UseAnd = true
                },

                ["AUD"] = new CurrencyInfo
                {
                    MajorName = "đô la Úc",
                    MinorName = "cent",
                    MinorDigits = 2,
                    UseAnd = true
                },

                ["GBP"] = new CurrencyInfo
                {
                    MajorName = "bảng Anh",
                    MinorName = "pence",
                    MinorDigits = 2,
                    UseAnd = false
                },

                ["CAD"] = new CurrencyInfo
                {
                    MajorName = "đô la Canada",
                    MinorName = "cent",
                    MinorDigits = 2,
                    UseAnd = true
                },

                ["CNY"] = new CurrencyInfo
                {
                    MajorName = "tệ",
                    MinorName = "xu",
                    MinorDigits = 2,
                    UseAnd = false
                }
            };

        #endregion

        #region đọc VND

        public static string DocTienBangChu(decimal soTien)
        {
            return DocTienTheoDonVi(soTien, "VND");
        }

        #endregion

        #region đọc theo đơn vị tiền tệ

        public static string DocTienTheoDonVi(decimal amount, string currencyCode)
        {
            if (!CurrencyMap.ContainsKey(currencyCode))
                throw new Exception("Currency not supported: " + currencyCode);

            var info = CurrencyMap[currencyCode];

            decimal rounded =
                Math.Round(amount, info.MinorDigits);

            long majorPart =
                (long)Math.Floor(rounded);

            long minorPart = 0;

            if (info.MinorDigits > 0)
            {
                minorPart =
                    (long)((rounded - majorPart)
                    * (decimal)Math.Pow(10, info.MinorDigits));
            }

            string result =
                ReadInteger(majorPart)
                + " " + info.MajorName;

            if (minorPart > 0)
            {
                string joinWord =
                    info.UseAnd ? " và " : " ";

                result += joinWord
                    + ReadInteger(minorPart)
                    + " " + info.MinorName;
            }

            result = result.Trim();

            return char.ToUpper(result[0])
                   + result.Substring(1);
        }

        #endregion

        #region đọc số nguyên

        private static string ReadInteger(long number)
        {
            if (number == 0)
                return "không";

            int maxIndex = 0;
            long temp = number;

            while (temp >= 1000)
            {
                temp /= 1000;
                maxIndex++;
            }

            string result = "";
            int i = 0;

            while (number > 0)
            {
                int block = (int)(number % 1000);

                if (block != 0)
                {
                    bool isHighestBlock = (i == maxIndex);

                    string text =
                        DocBaSo(block, isHighestBlock);

                    result =
                        text + " "
                        + Hang[i % 4]
                        + " "
                        + result;
                }

                number /= 1000;
                i++;
            }

            return result.Trim();
        }

        #endregion

        #region đọc 3 chữ số

        private static string DocBaSo(int number, bool isHighestBlock)
        {
            int tram = number / 100;
            int chuc = (number % 100) / 10;
            int donvi = number % 10;

            string result = "";

            // hàng trăm
            if (tram > 0)
            {
                result += ChuSo[tram] + " trăm";
            }
            else if (!isHighestBlock && number > 0)
            {
                result += "không trăm";
            }

            // hàng chục
            if (chuc > 1)
            {
                result += (result != "" ? " " : "") + ChuSo[chuc] + " mươi";

                if (donvi == 1)
                    result += "mốt ";
                else if (donvi == 4)
                    result += " tư ";
                else if (donvi == 5)
                    result += "lăm ";
                else if (donvi > 0)
                    result += ChuSo[donvi] + " ";
            }
            else if (chuc == 1)
            {
                result += (result != "" ? " " : "") + "mười";

                if (donvi == 5)
                    result += " lăm";
                else if (donvi > 0)
                    result += " " + ChuSo[donvi];
            }
            else if (donvi > 0)
            {
                if (tram > 0 || !isHighestBlock)
                    result += (result != "" ? " linh " : "linh ");

                result += ChuSo[donvi];
            }

            return result.Trim();
        }

        #endregion
    }
}