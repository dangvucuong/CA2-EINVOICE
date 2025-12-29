using Model.Base;

namespace Model.Table
{
	public class role_sub_system : modify_infor
	{
		public int id { get; set; }
		public int role_id { get; set; }
		public int sub_system_id { get; set; }
	}
}