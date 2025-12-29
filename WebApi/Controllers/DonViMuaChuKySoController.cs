using System.Threading.Tasks;
using Contract.Service;
using Microsoft.AspNetCore.Mvc;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/don-vi/chu-ky-so")]
    public class DonViMuaChuKySoController : BaseController
    {
        public DonViMuaChuKySoController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
        }
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] donvi_mua_chukyso model)
        {

            model.SetInsertInfo(0);
            model.tong_so_luong = model.so_luong + model.so_luong_khuyen_mai;
            model.id = await _serviceWrapper.Category.DonViMuaChuKySo.InsertAsync(model);
            if (model.id > 0)
            {
                await _serviceWrapper.Category.DonVi.SyncTotalChuKySoDaMuaAsync(model.donvi_mst);
                return this.OK(model.id);
            }
            return this.BadRequest();
        }
    }
}

