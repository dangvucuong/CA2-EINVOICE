using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/loai-hoa-don-ct")]
    [MustLogged]
    public class LoaiHoaDonCTController : BaseController
    {
        private ILoaiHoaDonCTService _loaiHoaDonCTService;
        public LoaiHoaDonCTController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._loaiHoaDonCTService = _serviceWrapper.HoaDon.LoaiHoaDonCT;
        }
        [HttpGet]
        public async Task<ContentResult> SelectAll()
        {
            var list = await _loaiHoaDonCTService.SelectAllAsync();
            list = list.Where(x => x.is_active).OrderBy(x => x.sort_idx).ToList();
            return this.OK(list);
        }
        [HttpGet]
        [Route("/api/loai-hoa-don/{loaiHoaDonId}/loai-hoa-don-ct")]
        public async Task<ContentResult> SelectByLoaHoaDonAsync(int loaiHoaDonId)
        {
            var list = await _loaiHoaDonCTService.SelectAllAsync();
            list = list.Where(x => x.is_active && x.loai_hoa_don_id == loaiHoaDonId).OrderBy(x => x.sort_idx).ToList();
            return this.OK(list);
        }
    }
}

