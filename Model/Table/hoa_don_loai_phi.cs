using Model.Base;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Table
{
    public class hoa_don_loai_phi : modify_infor
    {
        [SwaggerSchema(Description = "id tự tăng")]
        public int id { get; set; }
        [SwaggerSchema(Description = "Số thự tự")]
        public int stt { get; set; }
        [SwaggerSchema(Description = "id của hóa đơn")]
        public int hoa_don_id { get; set; }
         [SwaggerSchema(Description = "Tên lệ phí")]
        public string ten_le_phi { get; set; }
         [SwaggerSchema(Description = "Số tiền")]
        public decimal so_tien { get; set; }
    }
}