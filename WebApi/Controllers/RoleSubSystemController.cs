using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/role/{role_id}/sub-system")]
    [MustLogged]

    public class RoleSubSystemController : BaseController
    {
        public RoleSubSystemController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpGet]
        [MustAuthorized("[GET]api/role")]
        public async Task<ContentResult> SelectByRoleAsync(int role_id)
        {
            var list = await _serviceWrapper.User.RoleSubSystem.SelectByRoleAsync(role_id);
            return this.OK(list);
        }
        [HttpPost]
        [MustAuthorized("[PUT]api/role")]
        public async Task<ContentResult> AddSubSystemAsync(int role_id, [FromBody] role_sub_system model)
        {
            var user_id = this.GetUserId();
            model.role_id = role_id;
            model.SetInsertInfo(user_id);
            model.id = await _serviceWrapper.User.RoleSubSystem.InsertAsync(model);
            if (model.id > 0) return this.OK(model);
            return this.BadRequest();
        }
        [HttpDelete("{sub_system_id}")]
        [MustAuthorized("[PUT]api/role")]
        public async Task<ContentResult> RemoveSubsystemAsync(int role_id, int sub_system_id)
        {
            var list = await _serviceWrapper.User.RoleSubSystem.SelectByRoleAsync(role_id);
            var obj = list.Where(x => x.sub_system_id == sub_system_id).FirstOrDefault();
            if (obj == null) return this.BadRequest();
            var isDeleted = await _serviceWrapper.User.RoleSubSystem.DeleteAsync(obj.id);
            if (isDeleted) return this.OK();
            return this.BadRequest();
        }
    }
}

