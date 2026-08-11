using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.Pdf;
using Contracts.Service.TBSS;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using Model.Request.TBSS;
using Model.Request.ToKhai;
using Model.Respone.Upload;
using Model.Table;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/tbss")]


    public class ThongBaoSaiSotController : BaseController
    {
        private IPdfService _pdfService;
        private IThongBaoSaiSotService _thongBaoSaiSotService;
        public ThongBaoSaiSotController(IServiceWrapper serviceWrapper, IPdfService pdfService) : base(serviceWrapper)
        {
            this._thongBaoSaiSotService = _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSot;
            this._pdfService = pdfService;
        }
        [HttpGet]
        [MustAuthorized]
        [MustLogged]
        public async Task<ContentResult> SelectByDonViAsync()
        {
            var userInfo = this.GetUserInfo();
            var list = await _thongBaoSaiSotService.SelectByDonViAsync(userInfo.donvi_ma_dv);
            var ids = list.Select(x => x.id).ToList();
            var listChiTiet = await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdsAsync(ids);
            return this.OK(new
            {
                list = list,
                listChiTiet = listChiTiet
            });
        }
        [HttpGet("{id}")]
        [MustAuthorized("[GET]api/tbss")]
        [MustLogged]
        public async Task<ContentResult> SelectEditDataAsync(int id)
        {
            var userInfo = this.GetUserInfo();
            var obj = await _thongBaoSaiSotService.SelectByIdAsync(id);
            if (obj != null
            // && obj.donvi_ma_dv == userInfo.donvi_ma_dv
            )
            {
                var result = obj.Map<ThongBaoSaiSotAddOrEditRequest>();
                result.thong_bao_sai_sot_chi_tiets = (await _serviceWrapper.ThongBaoSaiSot.ThongBaoSaiSotChiTiet.SelectByThongBaoIdAsync(id)).ToList();
                return this.OK(result);
            }
            return this.BadRequest("Dữ liệu không hợp lệ");
        }
        [HttpGet("{id}/ky-so")]
        [MustAuthorized("[POST]api/tbss/phat-hanh")]
        [MustLogged]
        public async Task<ContentResult> XmlKySoBase64Async([FromRoute] int id)
        {
            var xml = await _thongBaoSaiSotService.CreateXmlKySoAsync(id);
            var base64 = xml.ConvertToBase64();
            return this.OK(base64);
        }
        [Route("phat-hanh")]
        [HttpPost]
        [MustLogged]
        [MustAuthorized]
        public async Task<ContentResult> PhatHanhAsync([FromBody] HoaDonPhatHanhRequest request)
        {
            var result = await _thongBaoSaiSotService.PhatHanhAsync(request.id, request.signed_text);
            return result.is_success ? this.OK() : this.BadRequest();
        }
        [HttpPost]
        [MustLogged]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] ThongBaoSaiSotAddOrEditRequest model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            var obj = model.Map<thong_bao_sai_sot>();
            obj.SetInsertInfo(user_id);
            obj.donvi_ma_dv = user.donvi_ma_dv;
            var result = await _thongBaoSaiSotService.SaveChangesAsync(model);
            if (result.is_success)
            {
                await this.SaveLogAsync($"Thêm thông báo sai sót {obj.id} với lý do: {obj.ly_do}", obj);
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpPut]
        [MustLogged]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] ThongBaoSaiSotAddOrEditRequest model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            var obj = model.Map<thong_bao_sai_sot>();
            obj.SetInsertInfo(user_id);
            obj.donvi_ma_dv = user.donvi_ma_dv;
            var result = await _thongBaoSaiSotService.SaveChangesAsync(model);
            if (result.is_success)
            {
                await this.SaveLogAsync($"Cập nhật thông báo sai sót {obj.id}", obj);
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpDelete("{id}")]
        [MustLogged]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _thongBaoSaiSotService.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            if (obj.thong_bao_sai_sot_trang_thai_id != (int)e_thong_bao_sai_sot_trang_thai.TAO_MOI) return this.BadRequest("Chỉ được xóa TBSS Nháp");
            var isDeleted = await _thongBaoSaiSotService.DeleteAsync(obj.id);
            if (isDeleted)
            {
                await this.SaveLogAsync($"Xóa thông báo sai sót: {obj.id}", null);
            }
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        [HttpPut("{id}/ky-so-remote")]
        [MustLogged]
        [MustAuthorized("[POST]api/tbss/phat-hanh")]
        public async Task<ContentResult> KySoRemote([FromRoute] int id)
        {
            var ketQua = await _thongBaoSaiSotService.KySoVaPhatHanhAsync(id);
            return ketQua.is_success ? this.OK() : this.BadRequest();
        }
        [HttpGet("{id}/html")]
        [MustLogged]
        [MustAuthorized("[GET]api/tbss")]
        public async Task<ContentResult> GetHtmlPreviewAsync([FromRoute] int id)
        {
            var html = await _thongBaoSaiSotService.GetHtmlPreviewAsync(id);
            return this.OK(html);
        }
        [HttpGet("{id}/ket-qua")]
        [MustLogged]
        [MustAuthorized("[GET]api/tbss")]
        public async Task<ContentResult> GetHtmlKetQuaAsync([FromRoute] int id)
        {
            var html = await _thongBaoSaiSotService.GetHtmlKetQuaAsync(id);
            return html.is_success ? this.OK(html.data) : this.BadRequest(html.message);
        }
        [HttpGet("{id}/download")]
        // [MustLogged]
        // [MustAuthorized("[GET]api/tbss")]
        public async Task<IActionResult> DownloadPdf([FromRoute] int id)
        {
            var html = await _thongBaoSaiSotService.GetHtmlPreviewAsync(id);
            var thongBaoSaiSot = await _thongBaoSaiSotService.SelectByIdAsync(id);
            var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
            var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
            {
                FileDownloadName = $"{"Thông báo sai sót vv"} {thongBaoSaiSot?.ly_do ?? ""}.pdf"
            };
            return fileContentResult;
        }
        [HttpPost]
        [Route("import/valid")]
        public async Task<ContentResult> ReadAndValidImportData([FromBody] UploadRespone upload)
        {
            var userInfo = this.GetUserInfo();
            var result = await this._thongBaoSaiSotService.ReadAndValidImportDataAsync(upload);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
    }
}

