using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.HoaDon
{
    public class MauHoaDonActiveRequest
    {
        [SwaggerSchema(Description = "id của mẫu hóa đơn")]

        public int id { get; set; }
        [SwaggerSchema(Description = "Sử dụng= true/ Ngừng sử dụng= false")]
        public bool is_active { get; set; }

    }
}