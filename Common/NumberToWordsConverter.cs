namespace Common
{
    using System;
    using System.Globalization;

    public class NumberToWordsConverter
    {
        private static readonly string[] Units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };

        private static string ReadDigit(int digit)
        {
            return Units[digit];
        }

        private static string ReadTens(int tens, int unit)
        {
            string result = "";
            if (tens == 0)
            {
                if (unit != 0)
                    result = "lẻ " + ReadDigit(unit);
            }
            else if (tens == 1)
            {
                result = "mười";
                if (unit != 0)
                    result += " " + (unit == 5 ? "lăm" : ReadDigit(unit));
            }
            else
            {
                result = ReadDigit(tens) + " mươi";
                if (unit != 0)
                    result += " " + (unit == 1 ? "mốt" : unit == 5 ? "lăm" : ReadDigit(unit));
            }
            return result;
        }

        private static string ReadHundreds(int hundreds, int tens, int unit)
        {
            string result = "";
            if (hundreds != 0)
                result = ReadDigit(hundreds) + " trăm";

            result += " " + ReadTens(tens, unit);
            return result.Trim();
        }

        private static string ReadGroup(int group)
        {
            int hundreds = group / 100;
            int tens = (group % 100) / 10;
            int unit = group % 10;
            return ReadHundreds(hundreds, tens, unit);
        }

        private static string ReadIntegerPart(long number)
        {
            if (number == 0) return "không";

            string[] units = { "", "nghìn", "triệu", "tỷ" };
            int unitIndex = 0;
            string result = "";

            while (number > 0)
            {
                int group = (int)(number % 1000);
                number /= 1000;

                if (group > 0)
                {
                    string groupText = ReadGroup(group);
                    if (unitIndex > 0)
                        groupText += " " + units[unitIndex];

                    result = groupText + " " + result;
                }
                unitIndex++;
            }
            return result.Trim();
        }

        public static string ConvertToWords(decimal amount, string currency)
        {
            long integerPart = (long)Math.Floor(amount);
            decimal decimalPart = amount - integerPart;

            string integerText = "";
            string decimalText = "";

            // Xử lý phần nguyên
            if (integerPart == 0)
            {
                if (currency.ToLower() == "usd")
                    integerText = "Không đô la mỹ";
                else if (currency.ToLower() == "eur")
                    integerText = "Không euro";
                else
                    integerText = "Không";
            }
            else
            {
                integerText = ReadIntegerPart(integerPart);
                if (currency.ToLower() == "usd")
                    integerText += " đô la mỹ";
                else if (currency.ToLower() == "eur")
                    integerText += " euro";
                else
                    integerText += " " + currency;
            }

            // Xử lý phần thập phân
            if (decimalPart > 0)
            {
                string decimalString = ((int)(decimalPart * 100)).ToString();
                decimalText = $"{decimalString} cents";
            }

            var result = (integerText + (decimalText != "" ? " và " + decimalText : "")).Trim();
            // Kiểm tra và loại bỏ "Lẻ" nếu chuỗi bắt đầu bằng từ này
            if (!string.IsNullOrEmpty(result))
            {
                if (result.ToUpper().StartsWith("LẺ"))
                {
                    result = result.Substring(2).Trim(); // Loại bỏ 2 ký tự đầu và xóa khoảng trắng dư
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                result = char.ToUpper(result[0], CultureInfo.InvariantCulture) + result.Substring(1);
            }
            return result;
        }
    }

}
