using System;
using System.Text;

public class NumberToWords
{
    private static readonly string[] Units = { "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
    private static readonly string[] PlaceValues = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ", "tỷ tỷ" };

    public static string ConvertToVietnameseWords(long number)
    {
        if (number == 0)
            return "Không đồng";

        string result = "";
        int placeIndex = 0;

        bool isNegative = number < 0;
        number = Math.Abs(number);

        while (number > 0)
        {
            int group = (int)(number % 1000);
            if (group > 0)
            {
                string groupText = ConvertThreeDigits(group);
                if (placeIndex > 0)
                    groupText += " " + PlaceValues[placeIndex];
                result = groupText + (string.IsNullOrEmpty(result) ? "" : " ") + result;
            }
            number /= 1000;
            placeIndex++;
        }

        result = result.Trim();
        if (isNegative)
            result = "Âm " + result;

        return char.ToUpper(result[0]) + result.Substring(1) + " đồng";
    }

    private static string ConvertThreeDigits(int number)
    {
        if (number == 0)
            return "";

        StringBuilder result = new StringBuilder();
        int hundreds = number / 100;
        int tens = (number % 100) / 10;
        int ones = number % 10;

        // Hàng trăm
        if (hundreds > 0)
        {
            result.Append(Units[hundreds] + " trăm");
            if (tens > 0 || ones > 0)
                result.Append(" ");
        }

        // Hàng chục
        if (tens > 1)
        {
            result.Append(Units[tens] + " mươi");
            if (ones > 0)
                result.Append(" ");
        }
        else if (tens == 1)
        {
            result.Append("mười");
            if (ones > 0)
                result.Append(" ");
        }
        else if (tens == 0 && ones > 0 && hundreds > 0)
        {
            result.Append("lẻ ");
        }

        // Hàng đơn vị
        if (ones > 0)
        {
            if (tens == 0 && ones == 5 && hundreds > 0)
                result.Append("lăm");
            else if (tens > 1 && ones == 5)
                result.Append("lăm");
            else if (tens > 1 && ones == 1)
                result.Append("mốt");
            else
                result.Append(Units[ones]);
        }

        return result.ToString().Trim();
    }

    // Hàm test
   
}