using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;
using Common;
using Contracts.Service.HoaDon;
using Model.Cache;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Service.HoaDon
{
    public class HoaDonSignService : IHoaDonSignService
    {
        private readonly IDatabase _redis;

        public HoaDonSignService(IDatabase redis)
        {
            _redis = redis;
        }

        // ===================================================================
        // PREPARE HASH - KHỚP CHUẨN THUỘC TÍNH SignedInfoHashBase64
        // ===================================================================
        public async Task<HoaDonPrepareHashSignResponse> PrepareCoreGenericAsync(XmlSignTarget target)
        {
            string sessionId = Guid.NewGuid().ToString();

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(target.XmlContent);

            // Gọi hàm Helper gốc của bạn (Trả về SignPrepareResult)
            SignPrepareResult prepareResult = XmlSignatureHelper.PrepareXmlSignature(
                doc,
                target.IdToSign,   // Tham số: idToSign
                target.ObjectId    // Tham số: objectId
            );

            // Lưu thông tin phiên vào Redis làm cache
            var sessionCache = new HoaDonSignSessionCache
            {
                HoaDonId = target.DocId,
                XmlContent = target.XmlContent,
                AppendXPath = target.AppendXPath,
                PrepareResultJson = JsonConvert.SerializeObject(prepareResult), // Lưu object chứa SignedInfoHashBase64
                IsCompleted = false
            };

            await _redis.StringSetAsync(
                $"sign-session:{sessionId}",
                JsonConvert.SerializeObject(sessionCache),
                TimeSpan.FromHours(1)
            );

            // 🟢 CHUẨN XÁC: Gọi đúng thuộc tính .SignedInfoHashBase64 từ class SignPrepareResult của bạn
            return new HoaDonPrepareHashSignResponse
            {
                SessionId = sessionId,
                HashBase64 = prepareResult.SignedInfoHashBase64
            };
        }

        // ===================================================================
        // FINALIZE HASH - RÁP CHỮ KÝ THEO XPATH ĐỘNG
        // ===================================================================
        public async Task<(string, int)> FinalizeCoreGenericAsync(HoaDonFinalizeHashSignRequest request,string certBase64,string appendXPath = "/HDon/DSCKS/NBan")
        {
            string sessionJson = await _redis.StringGetAsync($"sign-session:{request.SessionId}");
            if (string.IsNullOrEmpty(sessionJson))
            {
                throw new Exception($"Phiên ký số không tồn tại hoặc đã hết hạn: {request.SessionId}");
            }

            var session = JsonConvert.DeserializeObject<HoaDonSignSessionCache>(sessionJson);
            if (session.IsCompleted)
            {
                throw new Exception($"Phiên ký số này đã được hoàn thành trước đó: {request.SessionId}");
            }

            // Khôi phục chính xác đối tượng SignPrepareResult từ Cache
            var prepareResult = JsonConvert.DeserializeObject<SignPrepareResult>(session.PrepareResultJson);

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(session.XmlContent);

            // Khởi tạo Cert từ bộ nhớ RAM an toàn (Đã fix lỗi Iterations)
            byte[] certBytes = Convert.FromBase64String(certBase64);
            X509Certificate2 cert;
            try
            {
                cert = new X509Certificate2(certBytes, "", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi chuyển đổi chuỗi Cert Base64 thành X509Certificate2: " + ex.Message);
            }

            // Gọi hàm 5 tham số của XmlSignatureHelper truyền XPath động vào
            XmlSignatureHelper.FinalizeXmlSignature(doc,prepareResult,request.SignatureValue,cert,appendXPath );

            string signedXml = doc.OuterXml;

            // Kiểm tra tính toàn vẹn chữ ký số bằng hàm Verify gốc của bạn
            bool isValid = XmlSignatureHelper.VerifyXmlSignature(signedXml);
            if (!isValid)
            {
                throw new Exception($"Verify XML Signature lỗi trên bộ xác thực cho tài liệu ID: {session.HoaDonId}");
            }

            session.IsCompleted = true;
            await _redis.StringSetAsync(
                $"sign-session:{request.SessionId}",
                JsonConvert.SerializeObject(session),
                TimeSpan.FromHours(1)
            );
            return (signedXml, session.HoaDonId);
        }
    }
}