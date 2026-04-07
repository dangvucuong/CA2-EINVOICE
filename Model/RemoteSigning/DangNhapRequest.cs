using System.Text;

namespace Model.RemoteSigning
{
    public class DangNhapRequest : BaseRequest
    {
        private string _Mahethong;
        public DangNhapRequest()
        {

        }
        public DangNhapRequest(string Serial, string Masothue, string Mahethong, string Email)
        {
            this.Serial = Serial;
            this.Masothue = Masothue;
            this.Mahethong = Mahethong;
            this.Email = Email;
        }
        public override string Cleartext
        {
            get
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes("Login"));
            }
            set => throw new NotImplementedException();
        }
        public override string Keyhethong
        {
            get
            {
                return $"8|Login|{this.Serial}|Đăng nhập hệ thống Hóa đơn 78 Einvoice";
            }
            set => throw new NotImplementedException();
        }
        public override string Type
        {
            get
            {
                return "Text";
            }
            set => throw new NotImplementedException();
        }

        public override string Mahethong { get { return _Mahethong; } set { _Mahethong = value; } }
    }
}