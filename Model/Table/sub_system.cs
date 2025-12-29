using Model.Base;

namespace Model.Table
{
	public class sub_system : modify_infor
	{
		public int id { get; set; }
		public string short_name { get; set; }
		public string short_name_en { get; set; }
		public string name { get; set; }
		public string name_en { get; set; }
		public string icon { get; set; }
		public string path { get; set; }
		public bool is_active { get; set; }
		public string sort_idx { get; set; }
	}
}