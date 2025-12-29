using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.HoaDon;
using Microsoft.AspNetCore.Mvc;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/loai-hoa-don-ct-template")]
    [MustLogged]
    public class LoaiHoaDonCTTemplateController : BaseController
    {
        private ILoaiHoaDonCTTemplateService _loaiHoaDonCTTemplateService;
        public LoaiHoaDonCTTemplateController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._loaiHoaDonCTTemplateService = _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate;
        }
        [HttpGet]
        public async Task<ContentResult> SelectAll()
        {
            var list = await _loaiHoaDonCTTemplateService.SelectAllAsync();
            list = list.Where(x => x.is_active).ToList();
            return this.OK(list);
        }
        [HttpGet]
        [Route("/api/loai-hoa-don-ct/{loaiHoaDonCTId}/loai-hoa-don-ct-template")]
        public async Task<ContentResult> SelectByLoaHoaDonAsync(int loaiHoaDonCTId)
        {
            var list = await _loaiHoaDonCTTemplateService.SelectAllAsync();
            list = list.Where(x => x.is_active && x.loai_hoa_don_ct_id == loaiHoaDonCTId).ToList();
            return this.OK(list);
        }
        [HttpPost]
        [Route("preview")]
        public async Task<ContentResult> CreatePreviewDataAsync([FromBody] mau_hoa_don request)
        {
          
            var result = await _loaiHoaDonCTTemplateService.GeneratePreviewAsync(request);
            return this.OK(result);
        }
        

    }
}

