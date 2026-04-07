using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/menu")]
    [MustLogged]
    [MustAuthorized]
    public class MenuController : BaseController
    {
        public MenuController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpGet("sub-system/{sub_system_id}")]
        public async Task<ContentResult> GetAllAsync(int sub_system_id)
        {
            var list = await _serviceWrapper.User.Menu.SelectBySubSystemAsync(sub_system_id);
            return this.OK(list);
        }
    }
}

