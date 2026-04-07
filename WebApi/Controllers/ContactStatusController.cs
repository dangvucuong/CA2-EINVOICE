using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.Contact;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/contact-status")]
    [MustLogged]
    [MustAuthorized]
    public class ContactStatusController : BaseController
    {
        private IContactStatusService _contactStatusService;
        public ContactStatusController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._contactStatusService = _serviceWrapper.Contact.ContactStatus;
        }
        [HttpGet]

        public async Task<ContentResult> SelectAll()
        {
            var userInfo = this.GetUserInfo();
            var list = await _contactStatusService.SelectAllAsync();
            return this.OK(list);
        }

    }
}

