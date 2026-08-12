namespace Model.Respone.HoaDon
{
    public class HoaDonCreateXmlKySoRespone
    {
        public int id { get; set; }
        public string xml_base64 { get; set; }
        public bool is_success { get; set; } = true;
        public string message { get; set; }
    }
}