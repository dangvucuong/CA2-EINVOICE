using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Common
{
    public class XmlSignatureHelper
    {
        // =====================================================
        // PREPARE
        // =====================================================
        public static SignPrepareResult PrepareXmlSignature(XmlDocument doc, string idToSign, string objectId)
        {
            // GIỮ NGUYÊN DẤU GẠCH DƯỚI để tạo đúng cấu trúc giống hệt file mẫu của bạn
            // idToSign = "_1896600" -> sigId = "NBan_1896600", objId = "Obj-NBan_1896600"
            string cleanId = idToSign.StartsWith("_") ? idToSign.Substring(1) : idToSign;
            string sigId = $"NBan-{cleanId}";
            string objId = $"Obj-NBan-{cleanId}";

            // 1. Tìm node cần ký
            XmlElement nodeToSign = doc.SelectSingleNode("//*[@Id='" + idToSign + "']") as XmlElement;
            if (nodeToSign == null) throw new Exception($"Không tìm thấy node có Id='{idToSign}'");

            // 2. Khởi tạo Object thuộc đúng Namespace gốc để tính toán băm (Digest) đồng bộ context với bộ Verify của .NET
            XmlDocument objectDoc = new XmlDocument();
            objectDoc.PreserveWhitespace = true;
            objectDoc.LoadXml($@"<Object Id=""{objId}"" xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignatureProperties xmlns=""""><SignatureProperty Target=""#{sigId}""><SigningTime>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")}</SigningTime></SignatureProperty></SignatureProperties></Object>");

            // 3. Tính toán Digest cho Node cần ký
            byte[] digestNodeToSign = ComputeDigest(nodeToSign);

            // 4. Tính toán Digest cho Object
            byte[] digestObjectNode = ComputeDigest(objectDoc.DocumentElement);

            string digestValueNodeToSign = Convert.ToBase64String(digestNodeToSign);
            string digestValueObjectNode = Convert.ToBase64String(digestObjectNode);

            // 5. Tạo SignedInfo viết liền mạch một dòng
            string signedInfoXml =
                $@"<SignedInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">" +
                    $@"<CanonicalizationMethod Algorithm=""http://www.w3.org/TR/2001/REC-xml-c14n-20010315"" />" +
                    $@"<SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />" +
                    $@"<Reference URI=""#{idToSign}"">" +
                        $@"<Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /></Transforms>" +
                        $@"<DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />" +
                        $@"<DigestValue>{digestValueNodeToSign}</DigestValue>" +
                    $@"</Reference>" +
                    $@"<Reference URI=""#{objId}"">" +
                        $@"<DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />" +
                        $@"<DigestValue>{digestValueObjectNode}</DigestValue>" +
                    $@"</Reference>" +
                $@"</SignedInfo>";

            XmlDocument signedInfoDoc = new XmlDocument();
            signedInfoDoc.PreserveWhitespace = true;
            signedInfoDoc.LoadXml(signedInfoXml);

            // 6 & 7. Chuẩn hóa và băm SignedInfo lấy bytes hash chuẩn bị ký Token/HSM
            XmlDocument tempDoc = new XmlDocument();
            tempDoc.PreserveWhitespace = true;
            XmlNode importedSignedInfo = tempDoc.ImportNode(signedInfoDoc.DocumentElement, true);
            tempDoc.AppendChild(importedSignedInfo);

            var c14nTransform = new XmlDsigC14NTransform();
            c14nTransform.LoadInput(tempDoc);
            byte[] canonicalSignedInfo;
            using (Stream s = (Stream)c14nTransform.GetOutput(typeof(Stream)))
            using (MemoryStream ms = new MemoryStream())
            {
                s.CopyTo(ms);
                canonicalSignedInfo = ms.ToArray();
            }

            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(canonicalSignedInfo);
            }

            // Làm sạch chuỗi SignedInfo hiển thị thô trước khi đóng gói
            string cleanSignedInfoXml = signedInfoDoc.DocumentElement.OuterXml;
            cleanSignedInfoXml = cleanSignedInfoXml.Replace(@" xmlns=""http://www.w3.org/2000/09/xmldsig#""", "");

            // Xuất Object Xml nguyên bản ra ngoài kết quả trả về
            string cleanObjectXml = objectDoc.DocumentElement.OuterXml;

            return new SignPrepareResult
            {
                SignedInfoXml = cleanSignedInfoXml,
                SignedInfoHashBase64 = Convert.ToBase64String(hash),
                ObjectXml = cleanObjectXml,
                SignatureId = sigId
            };
        }

        // =====================================================
        // SIGN HASH
        // =====================================================
        public static string SignHash(string hashBase64, string certPath, string certPass)
        {
            byte[] hash = Convert.FromBase64String(hashBase64);

            X509Certificate2 cert = new X509Certificate2(certPath, certPass, X509KeyStorageFlags.Exportable);
            RSA rsa = cert.GetRSAPrivateKey();

            byte[] signatureValue;
            using (SHA256 sha256 = SHA256.Create())
            {
                signatureValue = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }

            return Convert.ToBase64String(signatureValue);
        }

        // =====================================================
        // FINALIZE
        // =====================================================
        public static void FinalizeXmlSignature(XmlDocument doc, SignPrepareResult prepareResult, string signatureBase64, X509Certificate2 cert, string appendXPath)
        {
            // ĐÃ BỔ SUNG: Thêm thẻ X509SubjectName lấy trực tiếp từ thuộc tính cert.Subject đúng chuẩn tệp hợp lệ
            string certBase64 = Convert.ToBase64String(cert.RawData);
            string keyInfoXml = $@"<KeyInfo><X509Data><X509SubjectName>{cert.Subject}</X509SubjectName><X509Certificate>{certBase64}</X509Certificate></X509Data></KeyInfo>";

            // 10. Dựng toàn bộ chuỗi XML của Node Signature bằng cách ghép chuỗi thô
            string rawSignatureNodeXml = $@"<Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"" Id=""{prepareResult.SignatureId}"">{prepareResult.SignedInfoXml}<SignatureValue>{signatureBase64}</SignatureValue>{keyInfoXml}{prepareResult.ObjectXml}</Signature>";

            // SỬA ĐỔI QUYẾT ĐỊNH: Chỉ định bóc tách chính xác chuỗi xmlns của xmldsig nằm trong thẻ Object một cách an toàn nhất
            string targetXmlns = @" xmlns=""http://www.w3.org/2000/09/xmldsig#""";
            int indexOfObject = rawSignatureNodeXml.IndexOf("<Object");
            if (indexOfObject >= 0)
            {
                // Tìm vị trí thuộc tính xmlns nằm bên trong thẻ Object và loại bỏ nó đi
                int indexOfXmlnsInObject = rawSignatureNodeXml.IndexOf(targetXmlns, indexOfObject);
                if (indexOfXmlnsInObject >= 0)
                {
                    rawSignatureNodeXml = rawSignatureNodeXml.Remove(indexOfXmlnsInObject, targetXmlns.Length);
                }
            }

            XmlDocument signatureDoc = new XmlDocument();
            signatureDoc.PreserveWhitespace = true;
            signatureDoc.LoadXml(rawSignatureNodeXml);

            // 11. Ghép nút Signature hoàn chỉnh vào nút cha trong tài liệu XML gốc thông qua ImportNode
            XmlElement appendNode = doc.SelectSingleNode(appendXPath) as XmlElement;
            if (appendNode == null)
            {
                throw new Exception("Không tìm thấy node append");
            }

            XmlNode importedSignature = doc.ImportNode(signatureDoc.DocumentElement, true);
            appendNode.AppendChild(importedSignature);
        }

        // =====================================================
        // COMPUTE DIGEST
        // =====================================================
        public static byte[] ComputeDigest(XmlElement element)
        {
            XmlDocument tempDoc = new XmlDocument();
            tempDoc.PreserveWhitespace = true;
            XmlNode importedNode = tempDoc.ImportNode(element, true);
            tempDoc.AppendChild(importedNode);

            XmlDsigC14NTransform c14n = new XmlDsigC14NTransform();
            c14n.LoadInput(tempDoc);

            byte[] canonicalBytes;
            using (Stream stream = (Stream)c14n.GetOutput(typeof(Stream)))
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                canonicalBytes = ms.ToArray();
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(canonicalBytes);
            }
        }

        // =====================================================
        // VERIFY
        // =====================================================
        public static bool VerifyXmlSignature(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xmlContent);

            SignedXml signedXml = new SignedXml(doc);
            XmlNode sigNode = doc.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl)[0];

            if (sigNode == null)
            {
                throw new Exception("Không tìm thấy Signature node");
            }

            signedXml.LoadXml((XmlElement)sigNode);
            return signedXml.CheckSignature();
        }
    }
}