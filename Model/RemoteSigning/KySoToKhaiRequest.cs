namespace Model.RemoteSigning
{
    public class KySoToKhaiRequest : BaseRequest
    {
        public int id { get; set; }
        public string ma_to_khai { get; set; }
        
        private string _Cleartext;


        public KySoToKhaiRequest()
        {

        }
        public KySoToKhaiRequest(string Serial, string Masothue, string base64Xml, string Email)
        {
            this.Serial = Serial;
            this.Masothue = Masothue;
            this.Email = Email;
            this.Cleartext = base64Xml;
        }
        public override string Cleartext
        {
            get
            {
                return _Cleartext;
            }
            set
            {
                _Cleartext = value;
            }
        }
        public override string Keyhethong
        {
            get
            {
                return $"{this.id.ToString()}|{this.ma_to_khai}|{this.Email}|Ký số tờ khai:{this.ma_to_khai}";
            }
            set => throw new NotImplementedException();
        }
        public override string Type
        {
            get
            {
                return "XML_HD";
            }
            set => throw new NotImplementedException();
        }

        public override string Mahethong
        {
            get
            {
                return "8";
            }
            set => throw new NotImplementedException();
        }
    }
}