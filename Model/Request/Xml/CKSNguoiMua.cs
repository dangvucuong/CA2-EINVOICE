using System.Xml.Serialization;

namespace Model.Request.Xml
{
    [XmlRoot(ElementName = "NMua")]
    public class CKSNguoiMua
    {
        [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public CKSSignature Signature { get; set; }
    }
}
