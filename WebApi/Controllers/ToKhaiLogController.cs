using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.ToKhai;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [MustLogged]
    public class ToKhaiLogController : BaseController
    {
        private IToKhaiLogService _toKhaiLogService;
        private IToKhaiService _toKhaiService;
        public ToKhaiLogController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._toKhaiLogService = _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog;
            this._toKhaiService = _serviceWrapper.ToKhaiSerivceWrapper.ToKhai;
        }
        /// <summary>
        /// tra cứu lịch sử của 1 tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet]
        [Route("api/to-khai/{id}/log")]
        public async Task<ContentResult> SelectByDonViAsync(int id)
        {
            var user = this.GetUserInfo();
            var obj = await _toKhaiService.SelectByIdAsync(id);
            if (obj == null || obj.donvi_ma_dv != user.donvi_ma_dv) return this.BadRequest();
            var list = await _toKhaiLogService.SelectByToKhaiAsync(id);
            return this.OK(list);
        }

    }
}

