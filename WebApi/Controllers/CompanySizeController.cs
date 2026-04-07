using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/company-size")]
    public class CompanySizeController : BaseController
    {
        private ICompanySizeService _companySizeService;
        public CompanySizeController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._companySizeService = _serviceWrapper.Contact.CompanySize;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ContentResult> SelectAll()
        {
            var userInfo = this.GetUserInfo();
            var list = await _companySizeService.SelectAllAsync();
            return this.OK(list);
        }
    }
}

