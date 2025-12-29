using Model.Base;

namespace Model.Table
{
	public class contact_status : modify_infor
	{
		public int id { get; set; }
		public string code { get; set; }
		public string name { get; set; }
	}
}