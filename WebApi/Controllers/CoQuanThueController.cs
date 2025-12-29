using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Base;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/co-quan-thue")]
    [MustLogged]
    public class CoQuanThueController : BaseController
    {
        private ICoQuanThueService _coQuanThueService;
        public CoQuanThueController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._coQuanThueService = _serviceWrapper.Category.CoQuanThue;
        }
        [HttpGet]
        public async Task<ContentResult> SelectByDonViAsync([FromQuery] PagingRequest? pagingRequest)
        {
            var list = await _coQuanThueService.SelectAsync(pagingRequest);
            return this.OK(list);
        }
        [HttpGet("{id}")]
        public async Task<ContentResult> SelectByIdAsync(int id)
        {
            var list = await _coQuanThueService.SelectByIdAsync(id);
            return this.OK(list);
        }


    }
}

