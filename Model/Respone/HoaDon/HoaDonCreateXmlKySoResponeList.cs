namespace Model.Respone.HoaDon
{
    public class HoaDonCreateXmlKySoResponeList
    {
        public int id { get; set; }
        public string xml_base64 { get; set; }
        public bool is_success { get; set; }
        public string? message { get; set; }
        public string? bien_ban_base64 { get; set; }
    }
}