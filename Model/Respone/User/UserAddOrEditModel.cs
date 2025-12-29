using Model.Table;

namespace Model.Respone.User
{
    public class UserEditModel:user
    {
        public UserEditModel(){
            this.role_ids = new List<int>();
        }
        public List<int> role_ids { get; set; }
    }
}