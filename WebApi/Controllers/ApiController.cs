using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/api")]
    [MustLogged]
    public class ApiController : BaseController
    {
        public ApiController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpGet("sub-system/{sub_system_id}")]
        public async Task<ContentResult> GetAllAsync(int sub_system_id)
        {
            var list = (await _serviceWrapper.User.Api.SelectAllViewModelAsync(sub_system_id)).ToList();
            if (!this.IsUserCanAccessApi("GET", "api/role/all"))
            {
                list = list.Where(x => !x.is_privte).ToList();
            }
            return this.OK(list);
        }
    }
}

