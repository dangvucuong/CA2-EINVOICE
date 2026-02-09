using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contract.Service.User;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Base;
using Model.Respone.User;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [MustLogged]

    public class UserController : BaseController
    {
        IUserService _userService;
        public UserController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._userService = _serviceWrapper.User.User;
        }
        // [HttpGet]
        // [MustAuthorized]
        // [Route("/api/user/all")]
        // public async Task<ContentResult> SelectAsync([FromQuery] PagingRequest? pagingRequest)
        // {
        //     var list = await _userService.SelectAsync(pagingRequest);
        //     return this.OK(list);

        // }
        [HttpGet]
        [MustAuthorized]
        [Route("/api/user")]
        public async Task<ContentResult> SelectAsync([FromQuery] PagingRequest? pagingRequest)
        {
            if (this.IsUserCanAccessApi("GET", "api/user/all"))
            {
                var list = await _userService.SelectAsync(pagingRequest);
                foreach (var item in list.data)
                {
                    item.password = string.Empty;
                }
                return this.OK(list);
            }
            else
            {

                var user = this.GetUserInfo();
                var list = await _userService.SelectByDonViAsync(user.donvi_ma_dv, pagingRequest);
                foreach (var item in list.data)
                {
                    item.password = string.Empty;
                }
                return this.OK(list);
            }


        }
        // [HttpGet]
        // [MustAuthorized]
        // [Route("/api/user/don-vi/{donvi_ma_dv}")]

        // public async Task<ContentResult> SelectByDonViAsync([FromQuery] PagingRequest? pagingRequest)
        // {
        //     var user = this.GetUserInfo();
        //     var list = await _userService.SelectByDonViAsync(user.donvi_ma_dv, pagingRequest);
        //     return this.OK(list);

        // }
        [HttpGet("{id}")]
        [MustAuthorized("[PUT]api/user")]
        public async Task<ContentResult> GetDetailAsync(int id)
        {
            var model = await _userService.SelectEditModelByIdAsync(id);
            if (model != null) model.password = "";
            return this.OK(model);

        }
        [HttpPut]
        public async Task<ContentResult> UpdateAsync([FromBody] UserEditModel model)
        {
            var isCanViewAll = this.IsUserCanAccessApi("GET", "api/user/all");
            var user = this.GetUserInfo();
            if (!isCanViewAll)
            {
                model.donvi_ma_dv = user.donvi_ma_dv;
            }
            //check mã đơn vị đã nằm trong đơn vị chưa
            var objDonVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(model.donvi_ma_dv);
            if (objDonVi == null) return this.BadRequest("MST không tồn tại trong danh sách đơn vị");
            if (model.serial_number.ConvertToString() != "")
            {
                var serials = objDonVi.serials.Split(";").Select(x => x.ToUpper().Trim()).ToList();
                if (serials.Where(x => x == model.serial_number.ConvertToString().ToUpper()).FirstOrDefault() == null)
                {
                    return this.BadRequest("Số Serial không tồn tại trong danh sách đơn vị");
                }
            }

            var validateResult = await _userService.ValidateUserExited(model);
            if (validateResult.is_success)
            {
                if (model.id <= 0 && model.password.ConvertToString() != string.Empty)
                {
                    model.password = model.password.GenerateBcrypt();
                }
                var saveResult = await _userService.SaveChangeAsync(model);
                if (saveResult.is_success)
                {
                    if (model.id > 0)
                    {
                        await this.SaveLogAsync($"Cập nhật user {model.username}", model);
                    }
                    else
                    {
                        await this.SaveLogAsync($"Thêm mới user {model.username}", model);
                    }
                    return this.OK(saveResult.data);
                }
                return this.BadRequest(saveResult.message);
            }
            return this.BadRequest(validateResult.message);

        }
        [HttpPut]
        [Route("remote-siging-serial")]
        public async Task<ContentResult> UpdateRemoteSigningSerial([FromBody] UserUpdateRemoteSigningSerialNumberRequest request)
        {
            request.SetUserId(this.GetUserId());
            var result = await _userService.UpdateRemoteSigningSerialAsync(request);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);

        }
        [HttpPut]
        [Route("serial")]
        public async Task<ContentResult> UpdateSerialNumerAsync([FromBody] UserUpdateSerialNumberRequest request)
        {
            request.SetUserId(this.GetUserId());
            var result = await _userService.UpdateSerialNumberAsync(request);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);

        }


        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _userService.SelectByIdAsync(id);
            if (obj == null)
                return this.BadRequest();

            // Gọi hàm mới dùng SP
            var result = await _userService.RemoveUserAsync(id);

            if (result.is_success)
            {
                await this.SaveLogAsync($"Xóa user: {obj.username}", null);
                return this.OK(result.message);
            }

            return this.BadRequest(result.message);
        }
    }


}

