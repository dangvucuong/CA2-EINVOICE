using Model.Base;

namespace Model.Table
{
    public class user_role : modify_infor
{
	public int id {get;set;}
	public int user_id {get;set;}
	public int role_id {get;set;}
}
}