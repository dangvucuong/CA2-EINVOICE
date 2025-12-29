using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/loai-hoa-don")]
    [MustLogged]
    public class LoaiHoaDonController : BaseController
    {
        private ILoaiHoaDonService _loaiHoaDonService;
        public LoaiHoaDonController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._loaiHoaDonService = _serviceWrapper.HoaDon.LoaiHoaDon;
        }
        [HttpGet]
        public async Task<ContentResult> SelectAll()
        {
            var list = await _loaiHoaDonService.SelectAllAsync();
            list= list.Where(x=>x.is_active).OrderBy(x=>x.sort_idx).ToList();
            return this.OK(list);
        }
    }
}

