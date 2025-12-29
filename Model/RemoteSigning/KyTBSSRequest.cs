namespace Model.RemoteSigning
{
    public class KyTBSSRequest : BaseRequest
    {
        public int id { get; set; }
        
        private string _Cleartext;


        public KyTBSSRequest()
        {

        }
        public KyTBSSRequest(string Serial, string Masothue, string base64Xml, string Email)
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
                return $"{this.id.ToString()}|0|{this.Email}|Ký số thông báo sai sót:{this.id}";
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