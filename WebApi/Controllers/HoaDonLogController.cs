using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [MustLogged]
    public class HoaDonLogController : BaseController
    {
        private IHoaDonLogService _hoaDonLogService;
        private IHoaDonService _hoaDonService;
        public HoaDonLogController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonLogService = _serviceWrapper.HoaDon.HoaDonLog;
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
        }
        [HttpGet]
        [Route("api/hoa-don/{id}/log")]
        public async Task<ContentResult> SelectByDonViAsync(int id)
        {
            var user = this.GetUserInfo();
            var obj = await _hoaDonService.SelectByIdAsync(id);
            if (obj == null || obj.donvi_ma_dv != user.donvi_ma_dv) return this.BadRequest();
            var list = await _hoaDonLogService.SelectByHoaDonAsync(id);
            list= list.OrderByDescending(x=>x.created_time).ToList();
            return this.OK(list);
        }

    }
}

