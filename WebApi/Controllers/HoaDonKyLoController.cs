using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Base;
using Model.Cache;
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
            var result = await _hoaDonKyLoService.CreateXmlVaPhatHanhsAsync(request, notify);
            return this.OK(result);
        }

        [HttpPost("prepare-hash-signs")]
        public async Task<IActionResult> PrepareHashSigns([FromBody] HoaDonDeletesRequest request)
        {
            var result = await _hoaDonKyLoService .PrepareHashSignsAsync(request.ids);
            return Ok(new SuccessResult<object>(result)
            );
        }

        [HttpPost("finalize-hash-sign")]
        public async Task<IActionResult> FinalizeHashSign([FromBody] HoaDonFinalizeHashSignRequest request)
        {

            var (signedXmlBase64, hoaDonId) = await _hoaDonKyLoService.FinalizeHashSignAsync(request);

            // 🟢 Dùng hoaDonId vừa lấy được từ Service (lấy từ Redis ra)
            return Ok(new
            {
                is_success = true,
                data = new
                {
                    hoaDonId = hoaDonId,
                    signedXmlBase64 = signedXmlBase64
                },
                message = "Success"
            });
        }    
    }
}

