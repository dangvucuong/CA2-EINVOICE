using System.Threading.Tasks;
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
    [Route("api/hang-hoa")]
    [MustLogged]

    public class HangHoaController : BaseController
    {
        private IHangHoaService _hangHoaService;
        public HangHoaController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hangHoaService = _serviceWrapper.Category.HangHoa;
        }
        [HttpGet]
        public async Task<ContentResult> SelectByDonViAsync([FromQuery] PagingRequest? pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _hangHoaService.SelectByDonViAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] dm_hanghoa model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;
            model.id = await _hangHoaService.InsertAsync(model);
            if (model.id > 0)
            {
                await this.SaveLogAsync($"Thêm hàng hóa mã: {model.ma_hang_hoa}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] dm_hanghoa model)
        {
            var user_id = this.GetUserId();
            var obj = await _hangHoaService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            obj.ma_hang_hoa = model.ma_hang_hoa;
            obj.ten_hang_hoa = model.ten_hang_hoa;
            obj.dvt = model.dvt;
            obj.ma_loai_hoang_hoa = model.ma_loai_hoang_hoa;
            obj.don_gia = model.don_gia;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _hangHoaService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Cập nhật hàng hóa mã: {model.ma_hang_hoa}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _hangHoaService.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            var isDeleted = await _hangHoaService.DeleteAsync(obj.id);
            if (isDeleted) await this.SaveLogAsync($"Xóa hàng hóa mã: {obj.ma_hang_hoa}", null);
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        [HttpPost]
        [Route("import/valid")]
        [MustAuthorized("[POST]api/khach-hang")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ReadAndValidImportData([FromBody] UploadRespone upload)
        {
            var result = await _serviceWrapper.Category.HangHoa.ReadAndValidImportDataAsync(upload);
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
            var result = await _serviceWrapper.Category.HangHoa.ImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
    }
}

