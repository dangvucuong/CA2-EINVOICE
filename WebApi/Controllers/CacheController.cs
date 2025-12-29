using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Core;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/cache")]
    [MustLogged]
    public class CacheController : BaseController
    {
        private ITaskQueueService _taskQueueService;
        public CacheController(IServiceWrapper serviceWrapper, ITaskQueueService taskQueueService) : base(serviceWrapper)
        {
            this._taskQueueService = taskQueueService;
        }
        [HttpGet("refresh")]
        public async Task<ContentResult> RefreshAsync()
        {
            await Task.WhenAll(
                               _serviceWrapper.Category.DonVi.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.Category.Watermark.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.Contact.CompanySize.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.Contact.ContactStatus.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),

                               _serviceWrapper.HoaDon.LoaiHoaDon.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.HoaDon.LoaiHoaDonCT.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),

                               _serviceWrapper.User.RoleApi.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.User.Api.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.User.Role.EnsureCachedDateUpdatedByLastUpdatTimeAsync(),
                               _serviceWrapper.User.Vender.EnsureCachedDateUpdatedByLastUpdatTimeAsync()
                           );
            return this.OK();

        }
    }
}

