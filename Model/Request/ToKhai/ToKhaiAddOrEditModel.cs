using Model.Table;
using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.ToKhai
{
    public class ToKhaiAddOrEditModel : to_khai
    {
        [SwaggerSchema(Description = "Danh sách chứng thư số")]
        public List<to_khai_cts> list_cts { get; set; }
        public ToKhaiAddOrEditModel()
        {
            this.list_cts = new List<to_khai_cts>();
        }
    }
}