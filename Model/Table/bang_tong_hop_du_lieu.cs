using Model.Base;

namespace Model.Table
{
	public class bang_tong_hop_du_lieu : modify_infor
	{
		public int id { get; set; }
		public string donvi_ma_dv { get; set; }
	
		public string bang_tong_hop_du_lieu_type { get; set; }
		public string ngay { get; set; }
		public int thang { get; set; }
		public int quy { get; set; }
		public int nam { get; set; }
		
		public string ky_du_lieu { get; set; }
		public bool is_lan_dau { get; set; }
		public int so_thu_tu_lan_bo_sung { get; set; }
		// public int so_bang_tong_hop_du_lieu { get; set; }
		public string phat_hanh_uuid { get; set; }
		public string user_id_phathanh { get; set; }
		public DateTime? thoi_gian_gui { get; set; }
		public int bang_tong_hop_du_lieu_trang_thai_id { get; set; }
		public int bang_tong_hop_du_lieu_loai_hang_hoa_id { get; set; }

		public string ket_qua_phat_hanh { get; set; }
		public int so_luong_hoa_don { get; set; }
	}
}