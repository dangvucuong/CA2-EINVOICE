using System;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using Model.Request.Contact;
using Model.Table;
using Service.Google;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/contact")]

    public class ContactController : BaseController
    {
        private readonly ReCaptchaService _reCaptchaService;

        private IContactService _contactService;
        public ContactController(IServiceWrapper serviceWrapper, ReCaptchaService reCaptchaService) : base(serviceWrapper)
        {
            this._contactService = _serviceWrapper.Contact.Contact;
            this._reCaptchaService = reCaptchaService;
        }
        [HttpGet]
        [MustLogged]
        [MustAuthorized]
        public async Task<ContentResult> SelectAll([FromQuery] ContactSelectRequest request)
        {
            var userInfo = this.GetUserInfo();
            var list = await _contactService.SelectAsync(request);
            return this.OK(list);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ContentResult> InsertAsync([FromBody] ContactAddRequest request)
        {
            var isValid = _reCaptchaService.IsValidate(request.reCaptchaToken);
            if (!isValid) return this.BadRequest();
            var model = request.Map<contact>();
            model.SetInsertInfo(0);
            model.register_at = DateTime.Now;
            model.contact_status_id = (int)e_contact_status.NEW;
            model.id = await _contactService.InsertAsync(model);
            if (model.id > 0)
            {

                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        [MustLogged]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] contact model)
        {
            var user_id = this.GetUserId();
            var obj = await _contactService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            obj.contact_status_id = model.contact_status_id;
            obj.note = model.note;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _contactService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Cập nhật thông tin contact {model.name}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }


    }
}

