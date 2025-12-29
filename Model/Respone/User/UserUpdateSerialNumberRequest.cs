using Model.Request.Base;

namespace Model.Respone.User
{
    public class UserUpdateSerialNumberRequest:HasUserIdRequest
    {
        public string serial { get; set; }
    }
}