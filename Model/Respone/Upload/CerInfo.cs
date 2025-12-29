namespace Model.Respone.Upload
{
    public class CerInfo
    {
        public string subject { get; set; }
        public string issuer { get; set; }
        public string thumbprint { get; set; }
        public DateTime not_before { get; set; }
        public DateTime not_after { get; set; }
        public string serial_number { get; set; }
        public string public_key { get; set; }
        public string signature_algorithm { get; set; }
        public string version { get; set; }
        public string extensions { get; set; }
    }
}