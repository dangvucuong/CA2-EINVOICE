using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Category;
using Model.Respone;
using Model.Table;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Filters;

namespace WebApi.Controllers
{
    /// <summary>
    /// API đồng bộ gói chữ ký số từ hệ thống HSDT.
    /// </summary>
    [ApiController]
    [Route("api/don-vi/chu-ky-so")]
    public class DonViMuaChuKySoController : BaseController
    {
        public DonViMuaChuKySoController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }

        /// <summary>
        /// Thêm gói chữ ký số đã mua
        /// </summary>
        /// <remarks>
        /// Dùng cho hệ thống HSDT đồng bộ gói CKS vào bảng donvi_mua_chukyso.
        ///
        /// **Phân quyền:** POST api/don-vi/chu-ky-so
        ///
        /// **Xác thực:** Header Authorization: Bearer {access_token} (lấy từ POST /api/account/login)
        ///
        /// **Xử lý:**
        /// - tong_so_luong = so_luong + so_luong_khuyen_mai
        /// - Đồng bộ lại tổng CKS còn lại của đơn vị
        ///
        /// **Response thành công:**
        /// ```
        /// { "is_success": true, "data": 15 }
        /// ```
        /// data là id bản ghi vừa thêm.
        /// </remarks>
        [HttpPost]
        [MustAuthorized]
        [SwaggerOperation(Tags = new[] { "HSDT - Chữ ký số" })]
        public async Task<ContentResult> InsertAsync([FromBody] donvi_mua_chukyso model)
        {

            model.SetInsertInfo(0);
            model.tong_so_luong = model.so_luong + model.so_luong_khuyen_mai;
            model.id = await _serviceWrapper.Category.DonViMuaChuKySo.InsertAsync(model);
            if (model.id > 0)
            {
                await _serviceWrapper.Category.DonVi.SyncTotalChuKySoDaMuaAsync(model.donvi_mst);
                return this.OK(model.id);
            }
            return this.BadRequest();
        }

        /// <summary>
        /// Thu hồi gói dịch vụ chữ ký số (ThuHoiGoiDv)
        /// </summary>
        /// <remarks>
        /// Dùng cho hệ thống HSDT thu hồi gói CKS theo mã số thuế và idHSDT.
        ///
        /// **Phân quyền:** dùng chung quyền POST api/don-vi/chu-ky-so (cùng API thêm gói).
        /// Tài khoản HSDT đã gọi được API thêm gói thì không cần gán quyền mới.
        ///
        /// **Xác thực:** Header Authorization: Bearer {access_token}
        ///
        /// **Xử lý:** Gọi SP donvi_mua_chukyso_thu_hoi
        /// (SET is_deleted=1, trang_thai_hd_id=0 WHERE donvi_mst + transaction_id).
        ///
        /// **Request mẫu:**
        /// ```
        /// { "ma_so_thue": "0103930279", "idHSDT": "HSDT-2026-001234" }
        /// ```
        ///
        /// **Response thành công (200):**
        /// ```
        /// {
        ///   "status_code": 200,
        ///   "is_success": true,
        ///   "message": "Thu hồi gói dịch vụ thành công",
        ///   "data": { "id": 15, "donvi_mst": "0103930279", "transaction_id": "HSDT-2026-001234" }
        /// }
        /// ```
        ///
        /// **Response thất bại (400):**
        /// - Mã số thuế không được để trống
        /// - idHSDT không được để trống
        /// - Không tìm thấy gói dịch vụ với mã số thuế và idHSDT tương ứng
        /// - Gói dịch vụ đã được thu hồi trước đó
        /// - Cập nhật dữ liệu thất bại
        ///
        /// **Không đủ quyền (401):** Token hết hạn hoặc role chưa có POST api/don-vi/chu-ky-so. Đăng nhập lại sau khi phân quyền.
        /// </remarks>
        [HttpPost]
        [Route("thu-hoi-goi-dv")]
        [MustAuthorized("[POST]api/don-vi/chu-ky-so")]
        [SwaggerOperation(Tags = new[] { "HSDT - Chữ ký số" })]
        public async Task<ContentResult> ThuHoiGoiDvAsync([FromBody] ThuHoiGoiDvRequest request)
        {
            var userId = this.GetUserId();
            var result = await _serviceWrapper.Category.DonViMuaChuKySo.ThuHoiGoiDvAsync(
                request?.ma_so_thue,
                request?.idHSDT,
                userId);

            if (result.is_success)
            {
                await _serviceWrapper.Category.DonVi.SyncTotalChuKySoDaMuaAsync(request.ma_so_thue.Trim());
                return new ResponeBaseSuccess(result.data, result.message).ToContentResult();
            }

            return this.BadRequest(result.message);
        }
    }
}

