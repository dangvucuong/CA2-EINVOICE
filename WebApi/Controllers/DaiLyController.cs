using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Base;
using Model.Table;
using WebApi.Filters;
using Common;
using Model.Respone.Upload;
using Model.Request.HoaDon;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/dai-ly")]
    [MustLogged]

    public class DaiLyController : BaseController
    {
        private IDaiLyService _daiLyService;
        public DaiLyController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._daiLyService = _serviceWrapper.Category.DaiLy;
        }
        [HttpGet]
        public async Task<ContentResult> SelectByDonViAsync([FromQuery] PagingRequest? pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _daiLyService.SelectByDonViAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] dai_ly model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;
            model.id = await _daiLyService.InsertAsync(model);
            if (model.id > 0)
            {
                await this.SaveLogAsync($"Thêm đại lý: {model.ten_dai_ly}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] dai_ly model)
        {
            var user_id = this.GetUserId();
            var obj = await _daiLyService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            obj.ma_dai_ly = model.ma_dai_ly;
            obj.ten_dai_ly = model.ten_dai_ly;
            obj.email = model.email;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _daiLyService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Cập nhật đại lý: {model.ten_dai_ly}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _daiLyService.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            var isDeleted = await _daiLyService.DeleteAsync(obj.id);
            if (isDeleted) await this.SaveLogAsync($"Xóa đại lý: {obj.ten_dai_ly}", null);
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        [HttpPost]
        [Route("import/valid")]
        [MustAuthorized("[POST]api/dai-ly")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ReadAndValidImportData([FromBody] UploadRespone upload)
        {
            var result = await _serviceWrapper.Category.DaiLy.ReadAndValidImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpPost]
        [Route("import")]
        [MustAuthorized("[POST]api/dai-ly")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ContentResult> ImportData([FromBody] HoaDonImportRequest upload)
        {
            var result = await _serviceWrapper.Category.DaiLy.ImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }

    }
}

