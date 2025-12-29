using System.Xml.Serialization;

namespace Model.Respone.Xml
{


    [XmlRoot(ElementName = "TTChung")]
    public class TTChung
    {

        [XmlElement(ElementName = "PBan")]
        public string PBan { get; set; }

        [XmlElement(ElementName = "MNGui")]
        public string MNGui { get; set; }

        [XmlElement(ElementName = "MNNhan")]
        public string MNNhan { get; set; }

        [XmlElement(ElementName = "MLTDiep")]
        public string MLTDiep { get; set; }

        [XmlElement(ElementName = "MTDiep")]
        public string MTDiep { get; set; }

        [XmlElement(ElementName = "MTDTChieu")]
        public string MTDTChieu { get; set; }

        [XmlElement(ElementName = "MST")]
        public string MST { get; set; }

        [XmlElement(ElementName = "SLuong")]
        public string SLuong { get; set; }
    }

    [XmlRoot(ElementName = "LDo")]
    public class LDo
    {

        [XmlElement(ElementName = "MLoi")]
        public int MLoi { get; set; }

        [XmlElement(ElementName = "MTLoi")]
        public string MTLoi { get; set; }

        [XmlElement(ElementName = "HDXLy")]
        public string HDXLy { get; set; }

        [XmlElement(ElementName = "MTa")]
        public string MTa { get; set; }


    }

    [XmlRoot(ElementName = "DSLDo")]
    public class DSLDo
    {

        [XmlElement(ElementName = "LDo")]
        public LDo LDo { get; set; }
    }

    [XmlRoot(ElementName = "MCCQT")]
    public class MCCQT
    {

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "HDon")]
    public class HDon
    {

        [XmlElement(ElementName = "KHMSHDon")]
        public string KHMSHDon { get; set; }

        [XmlElement(ElementName = "KHHDon")]
        public string KHHDon { get; set; }

        [XmlElement(ElementName = "SHDon")]
        public string SHDon { get; set; }

        [XmlElement(ElementName = "DSLDo")]
        public DSLDo DSLDo { get; set; }

        [XmlElement(ElementName = "MCCQT")]
        public MCCQT MCCQT { get; set; }

        [XmlElement(ElementName = "DSLDKTNhan")]
        public DSLDKTNhan DSLDKTNhan { get; set; }



        [XmlElement(ElementName = "TTTNCCQT")]
        public string TTTNCCQT { get; set; }




    }


    [XmlRoot(ElementName = "DSLDKTNhan")]
    public class DSLDKTNhan
    {

        [XmlElement(ElementName = "LDo")]
        public LDo LDo { get; set; }
    }

    [XmlRoot(ElementName = "DSHDon")]
    public class DSHDon
    {

        [XmlElement(ElementName = "HDon")]
        public HDon HDon { get; set; }
    }

    [XmlRoot(ElementName = "LHDKMa")]
    public class LHDKMa
    {

        [XmlElement(ElementName = "DSHDon")]
        public DSHDon DSHDon { get; set; }
    }


    [XmlRoot(ElementName = "KHLKhac")]
    public class KHLKhac
    {

        [XmlElement(ElementName = "DSLDo")]
        public DSLDo DSLDo { get; set; }
    }

    [XmlRoot(ElementName = "DSLDKCNhan")]
    public class DSLDKCNhan
    {

        [XmlElement(ElementName = "LDo")]
        public LDo LDo { get; set; }
    }

    [XmlRoot(ElementName = "DLTBao")]
    public class DLTBao
    {

        [XmlElement(ElementName = "PBan")]
        public string PBan { get; set; }

        [XmlElement(ElementName = "MSo")]
        public string MSo { get; set; }

        [XmlElement(ElementName = "Ten")]
        public string Ten { get; set; }

        [XmlElement(ElementName = "So")]
        public string So { get; set; }

        [XmlElement(ElementName = "DDanh")]
        public string DDanh { get; set; }

        // [XmlElement(ElementName = "NTBao")]
        // public DateTime NTBao { get; set; }

        [XmlElement(ElementName = "MST")]
        public string MST { get; set; }

        [XmlElement(ElementName = "TNNT")]
        public string TNNT { get; set; }

        // [XmlElement(ElementName = "TGGui")]
        // public DateTime TGGui { get; set; }

        [XmlElement(ElementName = "LTBao")]
        public string LTBao { get; set; }

        [XmlElement(ElementName = "CCu")]
        public string CCu { get; set; }

        [XmlElement(ElementName = "MGDDTu")]
        public string MGDDTu { get; set; }

        [XmlElement(ElementName = "SLuong")]
        public int SLuong { get; set; }

        [XmlElement(ElementName = "LHDKMa")]
        public LHDKMa LHDKMa { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "DSHDon")]
        public DSHDon DSHDon { get; set; }

        [XmlElement(ElementName = "KHLKhac")]
        public KHLKhac KHLKhac { get; set; }

        [XmlElement(ElementName = "DSLDKCNhan")]
        public DSLDKCNhan DSLDKCNhan { get; set; }

        [XmlElement(ElementName = "THop")]
        public string THop { get; set; }

         [XmlElement(ElementName = "TTXNCQT")]
        public string TTXNCQT { get; set; }

          [XmlElement(ElementName = "MCCQT")]
        public string MCCQT { get; set; }

        

        	
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

    [XmlRoot(ElementName = "DigestMethod")]
    public class DigestMethod
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string Algorithm { get; set; }
    }

    [XmlRoot(ElementName = "Reference")]
    public class Reference
    {

        [XmlElement(ElementName = "DigestMethod")]
        public DigestMethod DigestMethod { get; set; }

        [XmlElement(ElementName = "DigestValue")]
        public string DigestValue { get; set; }

        [XmlAttribute(AttributeName = "URI")]
        public string URI { get; set; }


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
        public DateTime SigningTime { get; set; }

        [XmlAttribute(AttributeName = "Target")]
        public string Target { get; set; }

        [XmlText]
        public DateTime Text { get; set; }
    }

    [XmlRoot(ElementName = "SignatureProperties")]
    public class SignatureProperties
    {

        [XmlElement(ElementName = "SignatureProperty")]
        public SignatureProperty SignatureProperty { get; set; }
    }

    [XmlRoot(ElementName = "Object")]
    public class SignatureObject
    {

        [XmlElement(ElementName = "SignatureProperties")]
        public SignatureProperties SignatureProperties { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

    }

    [XmlRoot(ElementName = "Signature")]
    public class Signature
    {

        [XmlElement(ElementName = "SignedInfo")]
        public SignedInfo SignedInfo { get; set; }

        [XmlElement(ElementName = "SignatureValue")]
        public string SignatureValue { get; set; }

        [XmlElement(ElementName = "KeyInfo")]
        public KeyInfo KeyInfo { get; set; }

        [XmlElement(ElementName = "Object")]
        public SignatureObject Object { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }


    }

    [XmlRoot(ElementName = "CQT")]
    public class CQT
    {

        [XmlElement(ElementName = "Signature")]
        public Signature Signature { get; set; }
    }

    [XmlRoot(ElementName = "DSCKS")]
    public class DSCKS
    {

        [XmlElement(ElementName = "CQT")]
        public CQT CQT { get; set; }
    }

    [XmlRoot(ElementName = "TBao")]
    public class TBao
    {

        [XmlElement(ElementName = "DLTBao")]
        public DLTBao DLTBao { get; set; }

        // [XmlElement(ElementName = "DSCKS")]
        // public DSCKS DSCKS { get; set; }
    }

    [XmlRoot(ElementName = "DLieu")]
    public class DLieu
    {

        [XmlElement(ElementName = "TBao")]
        public TBao TBao { get; set; }

        [XmlElement(ElementName = "HDon")]
        public HDon HDon { get; set; }
    }

    [XmlRoot(ElementName = "TDiep")]
    public class KetQuaThongDiepRespone
    {

        [XmlElement(ElementName = "TTChung")]
        public TTChung TTChung { get; set; }

        [XmlElement(ElementName = "DLieu")]
        public DLieu DLieu { get; set; }
    }


}