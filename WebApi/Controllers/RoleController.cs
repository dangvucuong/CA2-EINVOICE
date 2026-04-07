using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/role")]
    [MustLogged]
    public class RoleController : BaseController
    {
        public RoleController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpGet]
        public async Task<ContentResult> SelectAllAsync()
        {
            var user = this.GetUserInfo();
            if (this.IsUserCanAccessApi("GET", "api/role/all"))
            {
                var list = await _serviceWrapper.User.Role.SelectAllAsync();
                list = list.Where(x => x.donvi_ma_dv == user.donvi_ma_dv).ToList();
                return this.OK(list);
            }
            else
            {

                var list = (await _serviceWrapper.User.Role.SelectAllAsync()).Where(x => x.is_public || x.donvi_ma_dv == user.donvi_ma_dv).ToList();
                return this.OK(list);
            }

        }

        [HttpPost]
        public async Task<ContentResult> InsertAsync([FromBody] role model)
        {
            var user_id = this.GetUserId();
            var user = this.GetUserInfo();

            model.SetInsertInfo(user_id);
            // if (this.IsUserCanAccessApi("GET", "api/role/all") && model.is_public)
            // {
            //     model.donvi_ma_dv = string.Empty;
            // }
            // else
            // {
            //     model.donvi_ma_dv = user.donvi_ma_dv;
            // }
            model.donvi_ma_dv = user.donvi_ma_dv;

            model.id = await _serviceWrapper.User.Role.InsertAsync(model);
            if (model.id > 0)
            {
                await this.SaveLogAsync($"Thêm Role mới: {model.name}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        public async Task<ContentResult> UpdateAsync([FromBody] role model)
        {
            var user_id = this.GetUserId();
            var user = this.GetUserInfo();
            var obj = await _serviceWrapper.User.Role.SelectByIdAsync(model.id);
            if (obj == null && obj.donvi_ma_dv != user.donvi_ma_dv) return this.BadRequest();
            model.donvi_ma_dv = user.donvi_ma_dv;

            // if (this.IsUserCanAccessApi("GET", "api/role/all") && model.is_public)
            // {
            //     model.donvi_ma_dv = string.Empty;
            // }
            // else
            // {
            //     model.donvi_ma_dv = user.donvi_ma_dv;
            // }
            obj.description = model.description;
            obj.name = model.name;
            obj.name_en = model.name_en;
            obj.sort_idx = model.sort_idx;
            obj.is_active = model.is_active;
            obj.is_public = model.is_public;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _serviceWrapper.User.Role.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Cập nhật Role: {model.name}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _serviceWrapper.User.Role.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            var isDeleted = await _serviceWrapper.User.Role.DeleteAsync(obj.id);
            if (isDeleted)
            {
                await this.SaveLogAsync($"Xóa Role: {obj.name}", null);
            }
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }

    }
}

