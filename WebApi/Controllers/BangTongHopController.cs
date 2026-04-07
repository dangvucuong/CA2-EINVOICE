using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.BangTongHop;
using Microsoft.AspNetCore.Mvc;
using Model.Request.BangTongHop;
using Model.Request.ToKhai;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/bang-tong-hop")]
    [MustLogged]

    public class BangTongHopController : BaseController
    {
        private IBangTongHopService _bangTongHopService;
        public BangTongHopController(IServiceWrapper serviceWrapper) : base(serviceWrapper)
        {
            this._bangTongHopService = _serviceWrapper.BangTongHopDuLieu.BangTongHop;
        }
        [HttpGet]
        [MustAuthorized]
        public async Task<ContentResult> SelectByDonViAsync()
        {
            var userInfo = this.GetUserInfo();
            var list = await _bangTongHopService.SelectByDonViAsync(userInfo.donvi_ma_dv);
            return this.OK(list);

        }
        [HttpGet("{id}")]
        [MustAuthorized("[GET]api/bang-tong-hop")]
        public async Task<ContentResult> SelectEditDataAsync(int id)
        {
            var userInfo = this.GetUserInfo();
            var obj = await _bangTongHopService.SelectByIdAsync(id);
            if (obj != null && obj.donvi_ma_dv == userInfo.donvi_ma_dv)
            {
                var result = obj.Map<BangTongHopAddOrEditRequest>();
                result.hoa_don_ids = (await _serviceWrapper.BangTongHopDuLieu.BangTongHopHoaDon.SelectByBangTongHopAsync(id)).Select(x => x.hoa_don_id).ToList();
                result.hoa_dons = (await _serviceWrapper.HoaDon.HoaDon.SelectByIdsAsync(result.hoa_don_ids)).ToList();
                return this.OK(result);
            }
            return this.BadRequest("Dữ liệu không hợp lệ");
        }
        [HttpGet]
        [Route("{id}/log")]
        [MustAuthorized("[GET]api/bang-tong-hop")]
        public async Task<ContentResult> SelectLogAsync(int id)
        {
            var userInfo = this.GetUserInfo();
            var list = await _serviceWrapper.BangTongHopDuLieu.BangTongHopLog.SelectByBangTongHopIdAsync(id);
            return this.OK(list);
        }
        [HttpGet("{id}/ky-so")]
        [MustAuthorized("[POST]api/bang-tong-hop/phat-hanh")]
        public async Task<ContentResult> XmlKySoBase64Async([FromRoute] int id)
        {
            var xml = await _bangTongHopService.CreateXmlKySoAsync(id);
            var base64 = xml.ConvertToBase64();
            return this.OK(base64);
        }
        [Route("phat-hanh")]
        [MustAuthorized]
        [HttpPost]
        public async Task<ContentResult> PhatHanhAsync([FromBody] HoaDonPhatHanhRequest request)
        {
            var result = await _bangTongHopService.PhatHanhAsync(request.id, request.signed_text);
            return result.is_success ? this.OK() : this.BadRequest();
        }
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] BangTongHopAddOrEditRequest model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            // var obj = model.Map<bang_tong_hop_du_lieu>();
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;
            var result = await _bangTongHopService.SaveChangesAsync(model);

            if (result.is_success)
            {
                model.id = result.data.id;
                await this.SaveLogAsync($"Tạo bảng tổng hợp {model.id}", model);
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] BangTongHopAddOrEditRequest model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            var obj = model.Map<thong_bao_sai_sot>();
            obj.SetInsertInfo(user_id);
            obj.donvi_ma_dv = user.donvi_ma_dv;
            var result = await _bangTongHopService.SaveChangesAsync(model);
            if (result.is_success)
            {
                await this.SaveLogAsync($"Cập nhật thông báo sai sót {obj.id}", obj);
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var user = this.GetUserInfo();
            var obj = await _bangTongHopService.SelectByIdAsync(id);
            if (obj == null || obj.donvi_ma_dv != user.donvi_ma_dv || obj.bang_tong_hop_du_lieu_trang_thai_id != 1) return this.BadRequest();
            var isDeleted = await _bangTongHopService.DeleteAsync(obj.id);
            if (isDeleted)
            {
                await this.SaveLogAsync($"Xóa bảng tổng hợp: {obj.id}", null);
            }
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }

    }
}

