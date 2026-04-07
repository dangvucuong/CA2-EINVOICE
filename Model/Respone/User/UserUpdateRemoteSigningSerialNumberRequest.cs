using Model.Request.Base;

namespace Model.Respone.User
{
    public class UserUpdateRemoteSigningSerialNumberRequest:HasUserIdRequest
    {
        public string rs_ma_but_ky { get; set; }
    }
}