using Model.Base;

namespace Model.Table
{
	public class role : modify_infor
	{
		public int id { get; set; }
		public string name { get; set; }
		public string name_en { get; set; }
		public string description { get; set; }
		public bool is_active { get; set; }
		public string sort_idx { get; set; }
		public bool is_public { get; set; }
		public string? donvi_ma_dv { get; set; }
	}
}