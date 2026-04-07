using System.Xml.Serialization;

namespace Model.Request.Xml
{

    [XmlRoot(ElementName = "NBan")]
    public class CKSNguoiBan
    {

        [XmlElement(ElementName = "Signature", Namespace ="http://www.w3.org/2000/09/xmldsig#")]

        public CKSSignature Signature { get; set; }
        // 
    }

    [XmlRoot(ElementName = "CanonicalizationMethod")]
    public class CanonicalizationMethod
    {

        [XmlAttribute(AttributeName = "Algorithm")]

        public string Algorithm { get; set; }
    }

    [XmlRoot(ElementName = "SignatureMethod")]
    public class SignatureMethod
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string Algorithm { get; set; }
    }

    [XmlRoot(ElementName = "Transform")]
    public class Transform
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string Algorithm { get; set; }
    }

    [XmlRoot(ElementName = "Transforms")]
    public class Transforms
    {

        [XmlElement(ElementName = "Transform")]
        public Transform Transform { get; set; }
    }

    [XmlRoot(ElementName = "DigestMethod")]
    public class DigestMethod
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string Algorithm { get; set; }
    }

    [XmlRoot(ElementName = "Reference")]
    public class Reference
    {

        [XmlElement(ElementName = "Transforms")]
        public Transforms Transforms { get; set; }

        [XmlElement(ElementName = "DigestMethod")]
        public DigestMethod DigestMethod { get; set; }

        [XmlElement(ElementName = "DigestValue")]
        public string DigestValue { get; set; }

        [XmlAttribute(AttributeName = "URI")]
        public string URI { get; set; }

        // [XmlText]
        // public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SignedInfo")]
    public class SignedInfo
    {

        [XmlElement(ElementName = "CanonicalizationMethod")]
        public CanonicalizationMethod CanonicalizationMethod { get; set; }

        [XmlElement(ElementName = "SignatureMethod")]
        public SignatureMethod SignatureMethod { get; set; }

        [XmlElement(ElementName = "Reference")]
        public List<Reference> Reference { get; set; }
    }

    [XmlRoot(ElementName = "X509Data")]
    public class X509Data
    {

        [XmlElement(ElementName = "X509SubjectName")]
        public string X509SubjectName { get; set; }

        [XmlElement(ElementName = "X509Certificate")]
        public string X509Certificate { get; set; }
    }

    [XmlRoot(ElementName = "KeyInfo")]
    public class KeyInfo
    {

        [XmlElement(ElementName = "X509Data")]
        public X509Data X509Data { get; set; }
    }

    [XmlRoot(ElementName = "SignatureProperty")]
       
    public class SignatureProperty
    {

        [XmlElement(ElementName = "SigningTime")]
        public string SigningTime { get; set; }

        [XmlAttribute(AttributeName = "Target")]
        public string Target { get; set; }

        // [XmlText]
        // public DateTime Text { get; set; }
    }

    [XmlRoot(ElementName = "SignatureProperties")]
    public class SignatureProperties
    {

        [XmlElement(ElementName = "SignatureProperty")]
        public SignatureProperty SignatureProperty { get; set; }

        // [XmlAttribute(AttributeName = "xmlns")]
        // public string Xmlns { get; set; }

        // [XmlText]
        // public DateTime Text { get; set; }
    }

    [XmlRoot(ElementName = "Object")]
    public class ObjectData
    {

        [XmlElement(ElementName = "SignatureProperties")]
       
        public SignatureProperties SignatureProperties { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        // [XmlText]
        // public DateTime Text { get; set; }
    }

    // [XmlRoot(ElementName = "Signature")]

    public class CKSSignature
    {

        [XmlElement(ElementName = "SignedInfo")]
        public SignedInfo SignedInfo { get; set; }

        // [XmlElement(ElementName = "SignatureValue")]
        // public string SignatureValue { get; set; }

        // [XmlElement(ElementName = "KeyInfo")]
        // public KeyInfo KeyInfo { get; set; }

        [XmlElement(ElementName = "Object")]
        public ObjectData Object { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }
        public CKSSignature()
        {

        }

    }





}
