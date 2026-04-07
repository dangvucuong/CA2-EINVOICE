using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApp;

namespace Common
{
    public static class ReadNumberWebSerivce
    {
        public static string DocSoWebserviceEndpoint { get; private set; }
        public static void SetDocSoWebserviceEndpoint(string endpoint)
        {
            DocSoWebserviceEndpoint = endpoint;
        }
        public static async Task<string> DocSoAsync(decimal soTien, string loaiTien)
        {
            if (DocSoWebserviceEndpoint.ConvertToString() == "")
            {
                return NumberToWordsConverter.ConvertToWords(soTien, loaiTien);
            }
            var soTienString = soTien.ToString();
            var url = $"{DocSoWebserviceEndpoint}?Sotien={soTienString}&Loaitien={loaiTien}";
            try
            {
                // var soTienString = soTien.ToString();
                // var url = $"{DocSoWebserviceEndpoint}?Sotien={soTienString}&Loaitien={loaiTien}";
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    // Sử dụng Regex để tìm giá trị trong <string>
                    var match = Regex.Match(response, @"<string[^>]*>(.*?)<\/string>", RegexOptions.Singleline);

                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                    return string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                LogWriter.Writer("DocSoAsync Errro " + ex.Message, $"{url}", "");
                return NumberToWordsConverter.ConvertToWords(soTien, loaiTien);
                // return string.Empty;
            }
        }
    }
}