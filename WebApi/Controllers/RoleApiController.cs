using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [MustLogged]
    public class RoleApiController : BaseController
    {
        public RoleApiController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpGet]
        [Route("api/role/{role_id}/sub-system/{sub_sytem_id}/api")]
        [MustAuthorized("[GET]api/role,[GET]api/role-public")]
        public async Task<ContentResult> SelectByRoleAsync(int role_id, int sub_sytem_id)
        {
            var list = await _serviceWrapper.User.RoleApi.SelectByRoleAsync(role_id, sub_sytem_id);
            return this.OK(list);
        }
        [HttpPost]
        [Route("api/role/{role_id}/api")]
        [MustAuthorized("[PUT]api/role")]
        public async Task<IActionResult> AddApiAsync(int role_id, [FromBody] role_api model)
        {
            var user_id = this.GetUserId();
            var user = this.GetUserInfo();
            model.role_id = role_id;
            model.SetInsertInfo(user_id);
            var role = await _serviceWrapper.User.Role.SelectByIdAsync(role_id);
            if (role != null)
            {
                var isAllow = false;
                if (role.donvi_ma_dv == user.donvi_ma_dv) { isAllow = true; }
                if (role.is_public && this.IsUserCanAccessApi("GET", "api/role/all")) { isAllow = true; }
                if (!isAllow)
                {
                    return Unauthorized();
                }
            }
            model.id = await _serviceWrapper.User.RoleApi.InsertAsync(model);
            if (model.id > 0)
            {
                var api = await _serviceWrapper.User.Api.SelectByIdAsync(model.api_id);
                if (api != null && role != null)
                {
                    await this.SaveLogAsync($"Phân quyền chức năng {api.description} cho Role: {role.name}", model);
                }
                return this.OK(model);
            }
            return this.BadRequest();
        }

        [HttpDelete]
        [Route("api/role/{role_id}/api/{id}")]
        [MustAuthorized("[PUT]api/role")]

        public async Task<IActionResult> RemoveSubsystemAsync(int id)
        {
            var user = this.GetUserInfo();
            var roleApi = await _serviceWrapper.User.RoleApi.SelectByIdAsync(id);
            if (roleApi == null) return this.BadRequest();
            var role = await _serviceWrapper.User.Role.SelectByIdAsync(roleApi.role_id);
            if (role != null)
            {
                var isAllow = false;
                if (role.donvi_ma_dv == user.donvi_ma_dv) { isAllow = true; }
                if (role.is_public && this.IsUserCanAccessApi("GET", "api/role/all")) { isAllow = true; }
                if (!isAllow)
                {
                    return Unauthorized();
                }
            }
            var isDeleted = await _serviceWrapper.User.RoleApi.DeleteAsync(id);
            if (isDeleted)
            {

                var api = await _serviceWrapper.User.Api.SelectByIdAsync(roleApi?.api_id ?? 0);
                if (api != null && role != null)
                {
                    await this.SaveLogAsync($"Bỏ phân quyền chức năng {api.description} cho Role: {role.name}", null);
                }
                return this.OK();
            }
            return this.BadRequest();
        }
    }
}

