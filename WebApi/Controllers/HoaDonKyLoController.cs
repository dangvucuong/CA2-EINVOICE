using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Request.HoaDon;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/hoa-don-ky-lo")]
    [MustLogged]

    public class HoaDonKyLoController : BaseController
    {
        private IHoaDonService _hoaDonService;
        private IHoaDonKyLoService _hoaDonKyLoService;
        public HoaDonKyLoController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
            this._hoaDonKyLoService = _serviceWrapper.HoaDon.KyLo;
        }

        [Route("ky-so")]
        [HttpPost]
        [MustAuthorized("[POST]api/hoa-don")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> XmlKySoAsync([FromBody] HoaDonKyLoRequest request, [FromQuery] bool notify = true)
        {
            try
            {
                if (request?.ids == null || request.ids.Count == 0)
                    return this.BadRequest("Vui lòng chọn hóa đơn để ký số");

                var result = await _hoaDonKyLoService.CreateXmlVaPhatHanhsAsync(request, notify);
                return this.OK(result);
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(ex.Message.ConvertToString() != ""
                    ? ex.Message
                    : "Ký số phát hành thất bại");
            }
        }
    }
}

