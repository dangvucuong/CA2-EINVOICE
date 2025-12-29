using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
	public class hoa_don_dang_ky_phat_hanh : modify_infor
	{
		[SwaggerSchema(Description = "id tự tăng")]
		public int id { get; set; }
		[SwaggerSchema(Description = "Mã số thuế của đơn vị")]
		public string donvi_ma_dv { get; set; }
		[SwaggerSchema(Description = "Mẫu số")]
		public string mau_so { get; set; }
		[SwaggerSchema(Description = "Số lượng")]
		public int so_luong { get; set; }
		[SwaggerSchema(Description = "Số bắt đầu")]
		public string so_bat_dau { get; set; }
		[SwaggerSchema(Description = "Số kết thúc")]
		public string so_ket_thuc { get; set; }
		[SwaggerSchema(Description = "Ngày sử dụng")]
		public DateTime ngay_su_dung { get; set; }
		[SwaggerSchema(Description = "Số quyết định")]
		public string so_qd { get; set; }
		[SwaggerSchema(Description = "Ngày quyết định")]
		public DateTime ngay_qd { get; set; }
		[SwaggerSchema(Description = "Loại hóa đơn. Xem danh sách loại hóa đơn tại [GET] /api/loai-hoa-don-ct")]
		public int loai_hoa_don_ct_id { get; set; }
		[SwaggerSchema(Description = "Ký hiệu")]
		public string ky_hieu { get; set; }
		[SwaggerSchema(Description = "Tên hóa đơn")]
		public string ten_hoa_don { get; set; }
		[SwaggerSchema(Description = "Để mặc định=1")]
		public int hoa_don_dang_ky_phat_hanh_trang_thai_id { get; set; }
		[SwaggerSchema(Description = "Có mã: C, không mã: K, máy tính tiền: M")]
		public string hinh_thuc_code { get; set; }
		[SwaggerSchema(Description = "Có chịu thuế hay không")]
		public bool is_chiu_thue { get; set; }


	}
}