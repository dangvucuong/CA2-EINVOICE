using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.TBSS;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [MustLogged]
    public class ThongBaoSaiSotLogController : BaseController
    {
        private IThongBaoSaiSotLogService _thongBaoSaiSotLogService;
        private IThongBaoSaiSotService _thongBaoSaiSotService;
        public ThongBaoSaiSotLogController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._thongBaoSaiSotLogService = _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog;
            this._thongBaoSaiSotService = _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSot;
        }
        [HttpGet]
        [Route("api/tbss/{id}/log")]
        public async Task<ContentResult> SelectByDonViAsync(int id)
        {
            var user = this.GetUserInfo();
            var obj = await _thongBaoSaiSotService.SelectByIdAsync(id);
            if (obj == null || obj.donvi_ma_dv != user.donvi_ma_dv) return this.BadRequest();
            var list = await _thongBaoSaiSotLogService.SelectByThongBaoIdAsync(id);
            return this.OK(list);
        }

    }
}

