using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Respone.Upload;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/hoa-don-hang-hoa")]
    [MustLogged]

    public class HoaDonHangHoaController : BaseController
    {
        private IHoaDonHangHoaService _hoaDonHangHoaService;
        public HoaDonHangHoaController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._hoaDonHangHoaService = _serviceWrapper.HoaDon.HoaDonHangHoa;
        }
        [HttpPost]
        [Route("import/valid")]
        public async Task<ContentResult> ReadAndValidImportData([FromBody] UploadRespone upload)
        {
            var userInfo = this.GetUserInfo();
            var result = await this._hoaDonHangHoaService.ReadAndValidImportDataAsync(upload);
            if(result.is_success){
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }

    }
}

