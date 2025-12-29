namespace Model.RemoteSigning
{
    public class KySoRequest : BaseRequest
    {
        public int hoa_don_id { get; set; }
        public string ma_tra_cuu { get; set; }
        public string KHHDon { get; set; }
        public int so_hoa_don { get; set; }
        
        

        private string _Cleartext;


        public KySoRequest()
        {

        }
        public KySoRequest(string Serial, string Masothue, string base64Xml, string Email)
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
                return $"{this.hoa_don_id.ToString()}|{this.ma_tra_cuu}|{this.Email}|Ký số Hóa đơn 78:{this.KHHDon}_{this.so_hoa_don}";
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