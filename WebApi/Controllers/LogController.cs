using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.User;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Base;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/log")]
    [MustLogged]
    [MustAuthorized]
    public class LogController : BaseController
    {
        private ILogService _logService;
        public LogController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._logService = _serviceWrapper.User.Log;
        }
        [HttpGet]
        public async Task<ContentResult> SelectByDonViAsync([FromQuery] PagingRequest? pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _logService.SelectByDonViAsync(userInfo.donvi_ma_dv, pagingRequest);
            return this.OK(list);
        }
    }
}

