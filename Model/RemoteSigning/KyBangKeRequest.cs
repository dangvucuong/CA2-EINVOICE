namespace Model.RemoteSigning
{
    public class KyBangKeRequest : BaseRequest
    {
        public int so_luong { get; set; }



        private string _Cleartext;


        public KyBangKeRequest()
        {

        }
        public KyBangKeRequest(string Serial, string Masothue, string base64Xml, string Email)
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
                return $"{Masothue}|{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}|{this.Email}|Ký số bảng kê máy tính tiền (Số lượng {so_luong})";
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