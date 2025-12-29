using System.Xml.Serialization;
namespace Model.Request.HoaDon
{
    [XmlRoot(ElementName = "Root")]
    public class MauHoaDonCreateHtmlInput
    {
        [XmlElement(ElementName = "HDon")]
        public Model.Request.Xml.HoaDon hoa_don { get; set; }
    }
    

}