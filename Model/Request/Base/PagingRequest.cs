using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.Base
{
  public class PagingRequest
  {
    [SwaggerSchema(Description = "Số dòng trên 1 trang")]
    public int? page_size { get; set; }
    [SwaggerSchema(Description = "Số thứ tự trang (chạy từ 0)")]
    public int? page_index { get; set; }
    [SwaggerSchema(Description = "Sắp xếp theo field")]
    public string? sort_by { get; set; }
    [SwaggerSchema(Description = "Sắp xếp theo tăng dần=asc hoặc giảm dần = desc")]
    public string? sort_mode { get; set; }
    [SwaggerSchema(Description = "Tìm kiếm theo từ khóa")]
    public string? search_key { get; set; }
  }
}