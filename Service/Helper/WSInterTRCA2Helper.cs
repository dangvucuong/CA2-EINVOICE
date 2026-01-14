using System.ServiceModel;
using Model.Static;
using WSInterTRCA2;
using System.Xml;

namespace Service.Helper
{
    public static class WSInterTRCA2Helper
    {
        public static WSInterTRCA2SoapClient GetClient()
        {
            var epAddress = new System.ServiceModel.EndpointAddress(AppSettings.WSInterTRCA2Config.Endpoint);
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            // 200KB = 204800 bytes
            int limitSize = 204800;

            binding.MaxReceivedMessageSize = limitSize;
            binding.MaxBufferSize = limitSize;

            // QUAN TRỌNG:
            // Vì bạn nhận về chuỗi Base64 (thường là 1 chuỗi rất dài), 
            // bạn BẮT BUỘC phải tăng MaxStringContentLength.
            // Nếu không, dù MessageSize đủ nhưng độ dài chuỗi > 8KB sẽ vẫn lỗi.
            binding.ReaderQuotas = new XmlDictionaryReaderQuotas
            {
                MaxStringContentLength = limitSize, // Cho phép chuỗi dài tới 200KB
                MaxArrayLength = limitSize,         // Cho phép mảng byte dài tới 200KB

                // Các cái khác có thể để mặc định hoặc tăng nhẹ nếu cần
                MaxDepth = 32,
                MaxBytesPerRead = 4096,
                MaxNameTableCharCount = 16384
            };
            // --- HẾT CẤU HÌNH ---

            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var client = new WSInterTRCA2.WSInterTRCA2SoapClient(binding, epAddress);
            client.ClientCredentials.UserName.UserName = AppSettings.WSInterTRCA2Config.Username;
            client.ClientCredentials.UserName.Password = AppSettings.WSInterTRCA2Config.Password;
            return client;
        }
        public static AuthHeader GetAuthHeader()
        {
            var authHeader = new WSInterTRCA2.AuthHeader()
            {
                Password = AppSettings.WSInterTRCA2Config.Password,
                Username = AppSettings.WSInterTRCA2Config.Username
            };
            return authHeader;
        }
    }
}