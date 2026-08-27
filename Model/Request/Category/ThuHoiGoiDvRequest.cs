using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.Category
{
    public class ThuHoiGoiDvRequest
    {
        [SwaggerSchema(Description = "Mã số thuế đơn vị")]
        public string ma_so_thue { get; set; }

        [SwaggerSchema(Description = "Mã giao dịch HSDT (transaction_id)")]
        public string idHSDT { get; set; }
    }
}
