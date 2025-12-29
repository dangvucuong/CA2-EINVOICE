using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Notify;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/notify")]
    [MustLogged]
    public class NotifyController : BaseController
    {
        private INotifySerivce _notifySerivce;
        public NotifyController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._notifySerivce = _serviceWrapper.Notify.Notify;
        }
        [HttpGet]
        [Route("summary")]
        public async Task<ContentResult> SelectAll()
        {
            var userId = this.GetUserId();
            var list = await _notifySerivce.SelectNotifySummaryAsync(userId);
            return this.OK(list);
        }
    }
}

