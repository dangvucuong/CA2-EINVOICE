using System.Linq;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/watermark")]
    public class WatermarkTemplateController : BaseController
    {
        private IWatermarkService _watermarkService;
        public WatermarkTemplateController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._watermarkService = _serviceWrapper.Category.Watermark;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ContentResult> SelectAll()
        {
            var userInfo = this.GetUserInfo();
            var list = await _watermarkService.SelectAllAsync();
            list = list.OrderBy(x => x.id).ToList();
            return this.OK(list);
        }
    }
}

