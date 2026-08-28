using Swashbuckle.AspNetCore.Annotations;

namespace Model.Request.Category
{
    /// <summary>
    /// Request thu hồi gói dịch vụ chữ ký số từ hệ thống HSDT.
    /// </summary>
    public class ThuHoiGoiDvRequest
    {
        /// <summary>Mã số thuế đơn vị (cột donvi_mst).</summary>
        [SwaggerSchema(Description = "Mã số thuế đơn vị (cột donvi_mst)")]
        public string ma_so_thue { get; set; }

        /// <summary>Mã giao dịch HSDT, map với cột transaction_id.</summary>
        [SwaggerSchema(Description = "Mã giao dịch HSDT (cột transaction_id)")]
        public string idHSDT { get; set; }
    }
}
