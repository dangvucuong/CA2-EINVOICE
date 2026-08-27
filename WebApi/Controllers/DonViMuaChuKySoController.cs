using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Category;
using Model.Respone;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/don-vi/chu-ky-so")]
    public class DonViMuaChuKySoController : BaseController
    {
        public DonViMuaChuKySoController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpPost]
        [MustAuthorized]
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
        /// Thu hồi gói dịch vụ chữ ký số theo mã số thuế và idHSDT.
        /// Phân quyền: dùng chung quyền [POST]api/don-vi/chu-ky-so (cùng API thêm gói CKS từ HSDT).
        /// HSDT cần đăng nhập lấy Bearer token trước khi gọi.
        /// </summary>
        [HttpPost]
        [Route("thu-hoi-goi-dv")]
        [MustAuthorized("[POST]api/don-vi/chu-ky-so")]
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

