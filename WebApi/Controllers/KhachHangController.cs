using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Base;
using Model.Request.HoaDon;
using Model.Respone.Upload;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/khach-hang")]
    [MustLogged]

    public class KhachHangController : BaseController
    {
        private IKhachHangService _khachHangService;
        public KhachHangController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._khachHangService = _serviceWrapper.Category.KhachHang;
        }
        [HttpGet]
        public async Task<ContentResult> SelectByDonViAsync([FromQuery] PagingRequest? pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _khachHangService.SelectByDonViAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }

        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] khachhang model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;
            if (model.mst.ConvertToString() != "")
            {
                var checkExist = await _khachHangService.SelectByDonViAsync(model.donvi_ma_dv, model.mst);
                if (checkExist != null && checkExist.id != model.id)
                {
                    return this.BadRequest("Khách hàng đã tồn tại, không thể thêm mới");
                }
            }
            model.id = await _khachHangService.InsertAsync(model);
            if (model.id > 0)
            {
                await this.SaveLogAsync($"Thêm khách hàng: {model.ten_don_vi}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] khachhang model)
        {
            var user_id = this.GetUserId();
            var obj = await _khachHangService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            obj.ten_khach_hang = model.ten_khach_hang;
            obj.ten_don_vi = model.ten_don_vi;
            obj.dia_chi = model.dia_chi;
            obj.stk = model.stk;
            obj.mst = model.mst;
            obj.email = model.email;
            obj.ma_dv_ngan_sach = model.ma_dv_ngan_sach;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _khachHangService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Sửa khách hàng: {model.ten_don_vi}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _khachHangService.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            var isDeleted = await _khachHangService.DeleteAsync(obj.id);
            if (isDeleted)
            {
                await this.SaveLogAsync($"Xóa khách hàng: {obj.ten_don_vi}", null);
            }
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        [HttpPost]
        [Route("import/valid")]
        [MustAuthorized("[POST]api/khach-hang")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ReadAndValidImportData([FromBody] UploadRespone upload)
        {
            var result = await _serviceWrapper.Category.KhachHang.ReadAndValidImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("import")]
        [MustAuthorized("[POST]api/khach-hang")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ImportData([FromBody] HoaDonImportRequest upload)
        {
           var result = await _serviceWrapper.Category.KhachHang.ImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
    }
}

