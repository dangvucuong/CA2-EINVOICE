using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.ToKhai
{
    public class HoaDonPhatHanhRequest 
    {
        [SwaggerSchema(Description = "id của hóa đơn")]

        public int id { get; set; }
         [SwaggerSchema(Description = "base 64 của dữ liệu sau khi ký")]
        public string signed_text { get; set; }

        public string? bienBanSignedText { get; set; }
    }
}