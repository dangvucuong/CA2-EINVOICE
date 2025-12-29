using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/sub-system")]
    public class SubSytemController : BaseController
    {
        public SubSytemController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpGet]
        public async Task<ContentResult> GetAllAsync()
        {
            var list = await _serviceWrapper.User.SubSystem.SelectAllViewModelAsync();
            return list != null ? this.OK(list) : this.BadRequest();
        }
        [HttpGet]
        [Route("{id}/menu")]
        public async Task<ContentResult> GetMenuBySubSystemAsync(int id)
        {
            var list = await _serviceWrapper.User.Menu.SelectBySubSystemAsync(id);
            return this.OK(list);
        }
        [HttpGet]
        [Route("{id}/api")]
        public async Task<ContentResult> GetApiBySubSystem(int id)
        {
            var list = await _serviceWrapper.User.Api.SelectBySubSystemAsync(id);
            if (!this.IsUserCanAccessApi("GET", "api/role/all"))
            {
                list = list.Where(x => !x.is_privte).ToList();
            }
            return this.OK(list);
        }
    }
}

