using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Static;
using Service.Hub;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/app-config")]
    public class AppConfigController : BaseController
    {
        HoaDonPhatHanhHub _hoaDonPhatHanhHub;
        public AppConfigController(IServiceWrapper serviceWrapper, HoaDonPhatHanhHub hoaDonPhatHanhHub) : base(serviceWrapper)
        {
            this._hoaDonPhatHanhHub = hoaDonPhatHanhHub;
        }
        [HttpGet]
        public async Task<ContentResult> GetAsync()
        {
            //await _hoaDonPhatHanhHub.OnNewNotifyCreated(new Model.Request.Hub.HoaDonPhatHanhPushNotifyModel()
            //{
            //    file_thong_diep_url = "",
            //    hoa_don_trang_thai_id = 1,
            //    id = 1,
            //    ket_qua_phat_hanh = "test",
            //    user_id = "28057"
            //});

            return this.OK(new
            {
                ReCAPTCHASiteKey = AppSettings.GoogleRecaptcha.ClientID
            });
        }
    }
}

