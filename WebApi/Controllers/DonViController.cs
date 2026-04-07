using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Model.Request.Base;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/don-vi")]
    [MustLogged]

    public class DonViController : BaseController
    {
        private IDonViService _donViService;
        public DonViController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._donViService = _serviceWrapper.Category.DonVi;
        }
        [HttpGet]
        public async Task<ContentResult> SelectAllAsync([FromQuery] PagingRequest? pagingRequest)
        {
            var userInfo = this.GetUserInfo();
            var list = await _donViService.SelectAsync(pagingRequest);
            return this.OK(list);
        }
        [Route("lich-su-mua-cks")]
        [HttpGet]
        public async Task<ContentResult> SelectLichSuMuaChuKySo()
        {
            var userInfo = this.GetUserInfo();
            var list = await _serviceWrapper.Category.DonViMuaChuKySo.SelectByDonViAsync(userInfo.donvi_ma_dv);
            return this.OK(list);
        }
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] donvi model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            var checkExitst = await _donViService.SelectByMaDonViAsync(model.ma_dv);
            if (checkExitst != null)
            {
                return this.BadRequest("Đơn vị đã tồn tại");
            }
            model.SetInsertInfo(user_id);
            model.mst = model.ma_dv;
            // model.donvi_ma_dv = user.donvi_ma_dv;

            model.ten_dv = model.ten_dv.ConvertToString();
            model.dia_chi = model.dia_chi.ConvertToString();
            model.ten_dv = model.ten_dv.ConvertToString();
            model.dien_thoai = model.dien_thoai.ConvertToString();
            model.fax = model.fax.ConvertToString();
            model.website = model.website.ConvertToString();
            model.email = model.email.ConvertToString();
            model.stk = model.stk.ConvertToString();
            model.serials = model.serials.ConvertToString().RemoveAllSpace();
            model.ngan_hang = model.ngan_hang.ConvertToString();
            model.id = await _donViService.InsertAsync(model);
            if (model.id > 0)
            {
                await _donViService.SyncTotalChuKySoDaMuaAsync(model.ma_dv);
                await this.SaveLogAsync($"Thêm đơn vị: {model.ma_dv}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] donvi model)
        {
            var user_id = this.GetUserId();
            var obj = await _donViService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            obj.ten_dv = model.ten_dv;
            obj.dia_chi = model.dia_chi;
            obj.ten_dv = model.ten_dv;
            obj.dien_thoai = model.dien_thoai;
            obj.fax = model.fax;
            obj.website = model.website;
            obj.email = model.email;
            obj.stk = model.stk;
            obj.serials = model.serials.ConvertToString().RemoveAllSpace();
            obj.ngan_hang = model.ngan_hang;
            obj.ma_dang_ky_cqt = model.ma_dang_ky_cqt;
            obj.ngay_hoa_don_max = model.ngay_hoa_don_max;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _donViService.UpdateAsync(obj);
            if (isUpdated)
            {
                await _donViService.SyncTotalChuKySoDaMuaAsync(model.ma_dv);
                await this.SaveLogAsync($"Cập nhật đơn vị mã: {model.ma_dv}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        [Route("lien-he")]
        [HttpPut]
        public async Task<ContentResult> UpdateThongTinLienHeAsync([FromBody] donvi model)
        {
            var user_id = this.GetUserId();
            var user = this.GetUserInfo();
            var obj = await _donViService.SelectByIdAsync(model.id);
            if (obj == null || obj.ma_dv != user.donvi_ma_dv) return this.BadRequest();

            obj.dien_thoai = model.dien_thoai;
            obj.fax = model.fax;
            obj.website = model.website;
            obj.email = model.email;
            obj.stk = model.stk;
            obj.ngan_hang = model.ngan_hang;

            obj.ten_dv = model.ten_dv.ConvertToString();
            obj.dia_chi = model.dia_chi.ConvertToString();
            obj.co_quan_thu_id_chuquan = model.co_quan_thu_id_chuquan.ConvertToInt();
            obj.donvi_chuquan = model.donvi_chuquan.ConvertToString();
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _donViService.UpdateAsync(obj);
            if (isUpdated)
            {

                await this.SaveLogAsync($"Cập nhật thông tin liên hệ đơn vị mã: {model.ma_dv}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _donViService.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            var isDeleted = await _donViService.DeleteAsync(obj.id);
            if (isDeleted) await this.SaveLogAsync($"Xóa đơn vị mã: {obj.ma_dv}", null);
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        [HttpGet]
        [Route("gip/{mst}")]
        public async Task<ContentResult> GetGipInfoAsync([FromRoute] string mst)
        {
            var obj = await _donViService.GetGipInfoAsync(mst);
            return this.OK(obj);
        }

    }
}

