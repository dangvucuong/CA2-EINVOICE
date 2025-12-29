using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Model;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/don-vi/cts")]
    [MustLogged]

    public class DonViCtsController : BaseController
    {
        private IDonViCtsService _donViCtsService;
        public DonViCtsController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._donViCtsService = _serviceWrapper.Category.DonViCts;
        }
        [HttpGet]
        [MustAuthorized(BaseOnApis: "[PUT]api/user/ky-so")]
        public async Task<ContentResult> SelectByDonViAsync()
        {
            var userInfo = this.GetUserInfo();
            var list = await _donViCtsService.SelectByDonViAsync(userInfo.donvi_ma_dv);
            return this.OK(list);
        }

        [HttpPost]
        [MustAuthorized(BaseOnApis: "[PUT]api/user/ky-so")]
        public async Task<ContentResult> InsertAsync([FromBody] don_vi_cts model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;

            model.id = await _donViCtsService.InsertAsync(model);
            if (model.id > 0)
            {
                
                await this.SaveLogAsync($"Thêm CTS: {model.serial_number}", model);
                await _serviceWrapper.User.User.SyncFromCtsAsync(model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        [MustAuthorized(BaseOnApis: "[PUT]api/user/ky-so")]
        public async Task<ContentResult> UpdateAsync([FromBody] don_vi_cts model)
        {
            var user_id = this.GetUserId();
            var obj = await _donViCtsService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            obj.is_active = model.is_active;
            obj.serial_type_id = model.serial_type_id;
            obj.rs_ma_but_ky = model.rs_ma_but_ky;
            obj.serial_number = model.serial_number;
            obj.subject = model.subject;
            obj.issuer = model.issuer;
            obj.not_before = model.not_before;
            obj.not_after = model.not_after;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _donViCtsService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Sửa CTS: {model.serial_number}", model);
                await _serviceWrapper.User.User.SyncFromCtsAsync(model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        
        
    }
}

