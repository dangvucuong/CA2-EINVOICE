using System;
using System.Threading.Tasks;
using Contract.Service;
using Contracts.Service.ToKhai;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using Model.Request.ToKhai;
using WebApi.Filters;
using Common;
using Contracts.Service.Pdf;
using System.Linq;
using Model.Respone.ToKhai;
namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/to-khai")]
    [MustLogged]

    public class ToKhaiController : BaseController
    {
        private IToKhaiService _toKhaiService;
        private IPdfService _pdfService;
        public ToKhaiController(IServiceWrapper serviceWrapper, IPdfService pdfService) : base(serviceWrapper)
        {
            this._toKhaiService = _serviceWrapper.ToKhaiSerivceWrapper.ToKhai;
            this._pdfService = pdfService;
        }
        /// <summary>
        /// Danh sách tờ khai của đơn vị
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet]
        [MustAuthorized]
        public async Task<ContentResult> SelectByDonViAsync()
        {
            var userInfo = this.GetUserInfo();
            var list = await _toKhaiService.SelectByDonViAsync(userInfo.donvi_ma_dv);
            var toKhaiIds = list.Select(x => x.id).ToList();
            var listLog = await _serviceWrapper.ToKhaiSerivceWrapper.ToKhaiLog.SelectByToKhaiIdsAsync(toKhaiIds);
            var result = list.Select(x =>
            {
                var item = x.Map<ToKhaiVm>();
                if (item.to_khai_status_id == (int)e_to_khai_status.CQT_TU_CHOI)
                {
                    var log = listLog.Where(x => x.to_khai_id == x.to_khai_id && x.to_khai_log_type_id == 3).LastOrDefault();
                    item.ly_do_tu_choi = log?.noi_dung_thuc_hien ?? "";
                }
                return item;
            }).ToList();
            return this.OK(result);
        }
        /// <summary>
        /// Xem chi tiết 1 tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}")]
        [MustAuthorized("[GET]api/to-khai")]
        public async Task<ContentResult> SelectByIdAsync(int id)
        {
            var userInfo = this.GetUserInfo();
            var model = await _toKhaiService.SelectViewModel(id);
            return this.OK(model);
        }
        /// <summary>
        /// Thêm mới 1 tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [MustAuthorized]
        public async Task<ContentResult> InsertAsync([FromBody] ToKhaiAddOrEditModel model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.ngay_tao = DateTime.Now;
            model.nguoi_tao = user.full_name;
            model.SetInsertInfo(user_id);
            model.donvi_ma_dv = user.donvi_ma_dv;
            model.ma_dang_ky = Guid.NewGuid().ToString();
            var id = await _toKhaiService.SaveToKhaiAsync(model);
            if (id > 0)
            {
                await this.SaveLogAsync($"Thêm tờ khai: {model.ma_to_khai}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        [Route("ncm")]
        [HttpPost]
        [MustAuthorized]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ContentResult> InsertByNCMAsync([FromBody] ToKhaiAddOrEditModel model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.ngay_tao = DateTime.Now;
            model.nguoi_tao = user.full_name;
            model.donvi_ma_dv = model.donvi_ma_dv;
            model.to_khai_status_id = (int)e_to_khai_status.CQT_DONG_Y;
            // model.ma_dang_ky = Guid.NewGuid().ToString();
            model.SetInsertInfo(user_id);
            var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(model.donvi_ma_dv);
            if (donVi == null) return this.BadRequest("MST Đơn vị không tồn tại");
            var id = await _toKhaiService.SaveToKhaiAsync(model);
            if (id > 0)
            {
                await this.SaveLogAsync($"Thêm tờ khai: {model.ma_to_khai}", model);

                if (donVi != null)
                {
                    donVi.to_khai_success_id = id;
                    await _serviceWrapper.Category.DonVi.UpdateAsync(donVi);
                }
                return this.OK(model);
            }
            return this.BadRequest();
        }
        /// <summary>
        /// Update thông tin tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] ToKhaiAddOrEditModel model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;

            var obj = await _toKhaiService.SelectByIdAsync(model.id);
            if (obj == null || obj.to_khai_status_id != (int)e_to_khai_status.TAO_MOI || obj.donvi_ma_dv != user.donvi_ma_dv) return this.BadRequest();
            obj.SetUpdateInfo(user_id);
            var id = await _toKhaiService.SaveToKhaiAsync(model);
            if (id > 0)
            {
                await this.SaveLogAsync($"Sửa tờ khai: {model.ma_to_khai}", model);
                return this.OK(model);
            }
            return this.BadRequest();

        }
        /// <summary>
        /// Xóa tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _toKhaiService.SelectByIdAsync(id);
            if (obj == null || obj.to_khai_status_id != (int)e_to_khai_status.TAO_MOI) return this.BadRequest();
            var isDeleted = await _toKhaiService.DeleteAsync(obj.id);
            if (isDeleted) await this.SaveLogAsync($"Xóa tờ khai mã: {obj.ma_to_khai}", null);
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }
        /// <summary>
        /// Lấy base 64 của tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}/ky-so")]
        [MustAuthorized("[POST]api/to-khai/phat-hanh")]
        public async Task<ContentResult> XmlKySoBase64Async([FromRoute] int id)
        {
            var xml = await _toKhaiService.CreateXmlKySoAsync(id);
            var base64 = xml.ConvertToBase64();
            return this.OK(base64);
        }
        /// <summary>
        /// In tờ khai thành html
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}/print")]
        [MustAuthorized("[GET]api/to-khai")]
        public async Task<ContentResult> GetHtmlPrintAsync([FromRoute] int id)
        {
            var html = await _toKhaiService.GetHtmlPrintAsync(id);
            if (html.is_success)
            {
                var cacheKey = $"PRINT_TO_KHAI_{id}";
                await _serviceWrapper.Cache.SetDataAsync<string>(cacheKey, html.data, DateTime.Now.AddHours(1));
            }
            return html.is_success ? this.OK(html.data) : this.BadRequest(html.message);
        }
        [HttpGet("{id}/print/ket-qua")]
        [MustAuthorized("[GET]api/to-khai")]
        public async Task<ContentResult> GetHtmlPhatHanhAsync([FromRoute] int id)
        {
            var html = await _toKhaiService.GetHtmlPhatHanhAsync(id);
            if (html.is_success)
            {
                var cacheKey = $"PRINT_TO_KHAI_PHAT_HANH_{id}";
                await _serviceWrapper.Cache.SetDataAsync<string>(cacheKey, html.data, DateTime.Now.AddHours(1));
            }
            return html.is_success ? this.OK(html.data) : this.BadRequest(html.message);
        }
        /// <summary>
        /// Tải tờ khai dạng pdf
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}/pdf")]
        [MustAuthorized("[GET]api/to-khai")]
        public async Task<IActionResult> GetPdfFileAsync([FromRoute] int id)
        {
            var html = "";
            var cacheKey = $"PRINT_TO_KHAI_{id}";
            html = await _serviceWrapper.Cache.GetDataAsync<string>(cacheKey);
            html = html.ConvertToString();
            if (html == string.Empty)
            {
                var htmlResult = await _toKhaiService.GetHtmlPrintAsync(id);
                if (htmlResult.is_success) html = htmlResult.data;
            }
            var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
            var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
            {
                FileDownloadName = $"To-khai-{id.ToString()}.pdf"
            };
            return fileContentResult;
        }
        /// <summary>
        /// Tải tờ khai kết quả dạng pdf
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}/pdf/ket-qua")]
        [MustAuthorized("[GET]api/to-khai")]
        public async Task<IActionResult> GetPdfFilePhatHanhAsync([FromRoute] int id)
        {
            var html = "";
            var cacheKey = $"PRINT_TO_KHAI_PHAT_HANH_{id}";
            html = await _serviceWrapper.Cache.GetDataAsync<string>(cacheKey);
            html = html.ConvertToString();
            if (html == string.Empty)
            {
                var htmlResult = await _toKhaiService.GetHtmlPhatHanhAsync(id);
                if (htmlResult.is_success) html = htmlResult.data;
            }
            var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
            var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
            {
                FileDownloadName = $"To-khai-ket-qua-{id.ToString()}.pdf"
            };
            return fileContentResult;
        }
        /// <summary>
        /// Phát hành tờ khai
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [Route("phat-hanh")]
        [MustAuthorized]
        public async Task<ContentResult> PhatHanhAsync([FromBody] HoaDonPhatHanhRequest request)
        {
            await _toKhaiService.PhatHanhAsync(request.id, request.signed_text);
            return this.OK();
        }
        [HttpPut("{id}/ky-so-remote")]
        [MustAuthorized("[POST]api/tbss/phat-hanh")]
        public async Task<ContentResult> KySoRemote([FromRoute] int id)
        {
            var ketQua = await _toKhaiService.KySoVaPhatHanhAsync(id);
            return ketQua.is_success ? this.OK() : this.BadRequest();
        }

    }
}

