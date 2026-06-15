using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Contracts.Service.Pdf;
using Microsoft.AspNetCore.Mvc;
using Model.Request.HoaDon;
using Model.Respone.MauHoaDon;
using Model.Table;
using Service.HoaDon;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/mau-hoa-don")]
    [MustLogged]

    public class MauHoaDonController : BaseController
    {
        private IMauHoaDonService _mauHoaDonService;
        private IPdfService _pdfService;
        public MauHoaDonController(IServiceWrapper serviceWrapper, IPdfService pdfService) : base(serviceWrapper)
        {
            this._mauHoaDonService = _serviceWrapper.HoaDon.MauHoaDon;
            this._pdfService = pdfService;
        }
        /// <summary>
        /// Xem danh sách mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet]
        [MustAuthorized]
        public async Task<ContentResult> SelectByDonViAsync()
        {
            var userInfo = this.GetUserInfo();
            var list = await _mauHoaDonService.SelectByDonViAsync(userInfo.donvi_ma_dv);
            return this.OK(list);
        }
        /// <summary>
        /// Xem chi tiết 1 mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpGet("{id}")]
        [MustAuthorized("[GET]api/mau-hoa-don")]
        public async Task<ContentResult> SelectByIdAsync(int id)
        {
            var userInfo = this.GetUserInfo();
            var obj = await _mauHoaDonService.SelectByIdAsync(id);
            if (obj != null && obj.donvi_ma_dv == userInfo.donvi_ma_dv)
            {
                return this.OK(obj);
            }
            return this.BadRequest();
        }
        /// <summary>
        /// Tải pdf của 1 mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [MustAuthorized("[GET]api/mau-hoa-don")]
        [HttpGet("{id}/pdf")]

        public async Task<IActionResult> DownloadPreviewPdfAsync(int id)
        {
            var mauHoaDon = await _mauHoaDonService.SelectByIdAsync(id);
            if (mauHoaDon == null) return null;
            var html = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.GeneratePreviewAsync(mauHoaDon);
            //
            var bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;position: relative;";
            bgstyle = bgstyle + "background-image: url('{paramWaterMark}'); background-size:80%; background-position: center;background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;background-repeat:no-repeat";
            var noidungdisabled = "&#160;";
            var styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            var stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";

            styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            html = html.Replace("paramSign", "display:none");

            var paramsubtitle = "normal";
            // var paramsubtitle = "none";
            var paramSubtitleDiv = "none";
            var paramsubtitlecontent = "";
            var paramSubtitleContentDiv = "&#160;";


            html = html.Replace("viewstyle", bgstyle)
            // .Replace("paramLogo", "{paramLogo}")
           .Replace("paramChuyendoi", "display:none")
           .Replace("paramMau", stylemau)
            .Replace("height: auto; min-height: 100%;", "height: auto;")
           .Replace("paramNguoiCD", "width:100%;text-align:center;display:none")
           .Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled)
           .Replace("param1_1", paramsubtitle)
           .Replace("param1", paramsubtitlecontent)
           .Replace("param2_2", paramSubtitleDiv)
           .Replace("param2", paramSubtitleContentDiv)
           .Replace("paramdisable", styledisabled)
           .Replace("contentDisable", noidungdisabled)
           .Replace("paramlien", "0").Replace("paramdisplay", "display:none");

            if (mauHoaDon.is_show_wattermark_inner_table == true)
            {
                html = html.Replace("{paramLogo}", mauHoaDon.logo_path.ConvertToString().Replace('\\', '/') ?? "")
                                .Replace("paramWaterMarkTable;", mauHoaDon.watermark_path.ConvertToString().Replace('\\', '/') ?? "");
            }
            else
            {
                html = html.Replace("{paramLogo}", mauHoaDon.logo_path?.ConvertToString().Replace('\\', '/') ?? "")
                                .Replace("{paramWaterMark}", mauHoaDon.watermark_path?.ConvertToString().Replace('\\', '/') ?? "");
            }
            if (mauHoaDon.vien_path.ConvertToString() != "")
            {
                html = html.Replace("{paramVien}", mauHoaDon.vien_path?.ConvertToString().Replace('\\', '/') ?? "");

            }
            if (mauHoaDon.logo_position.ConvertToString() == "right")
            {
                html = html.Replace("paramOpacityHeaderFlexDirection;", "row-reverse");
            }
            var paramOpacity = (1 - (mauHoaDon.watermark_opacity * 1.0 / 100).ConvertToDouble(2)).ToString().Replace(",", ".");
            html = html.Replace("paramOpacity;", paramOpacity);
            html = html.Replace("paramOpacity;", paramOpacity);
            var advancedSettings = mauHoaDon.advanced_settings_json.ConvertToString().TryDeserializeObject<CssEditorElementData[]>();
            html = html.Replace("12pt", "12px");
            html = html.Replace("<table style=\"width:100%;line-height:25px;font-size:12pt\">", "<table style=\"width:100%;line-height:20px;font-size:12px\">");
            html = html.Replace("line-height:25px", "line-height:20px");
            foreach (var ad in advancedSettings)
            {
                var keyCss = $"{ad.elementId}_css;";
                var keyCssDisplay = $"{ad.elementId}_css_display;";
                var css = new List<string>()
                {
                    $"font-weight:{(ad.cssValue?.isBold==true ? "bold" : "normal")}",
                    $"font-style:{(ad.cssValue?.isItalic==true ? "italic" : "normal")}",
                    $"font-size:{ad.cssValue?.fontSize}px",
                    $"color:{ad.cssValue?.color}",
                    $"text-align:{ad.cssValue?.align}"
                }.Join(";");
                html = html.Replace(keyCss, css);
                html = html.Replace(keyCssDisplay, ad.isDisplay ? "" : "display:none");
            }
            //
            var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
            var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
            {
                FileDownloadName = $"{mauHoaDon.name.ToString().ConvertToNoAccents()}.pdf"
            };
            return fileContentResult;
        }

        /// <summary>
        /// Thêm mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPost]
        [MustAuthorized]

        public async Task<ContentResult> InsertAsync([FromBody] mau_hoa_don model)
        {
            var user = this.GetUserInfo();
            var user_id = user.id;
            model.SetInsertInfo(user_id);
            model.ngay_qd = model.ngay_qd?.Date ?? null;
            model.donvi_ma_dv = user.donvi_ma_dv;
            model.is_active = false;
            //
            var mauHoaDonTemplate = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectVmByIdAsync(model.loai_hoa_don_ct_template_id);
            if (mauHoaDonTemplate != null)
            {
                var sourceFile = MauHoaDonService.ResolveContentPath(mauHoaDonTemplate.path);
                if (!System.IO.File.Exists(sourceFile))
                    return this.BadRequest("Không tìm thấy file template XSLT");

                var destFile = MauHoaDonService.BuildTemplateDestPath(model.donvi_ma_dv);
                var destinationDirectory = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                System.IO.File.Copy(sourceFile, destFile, true);
                model.xslt_path = MauHoaDonService.ToRelativeContentPath(destFile);
            }
            //
            model.id = await _mauHoaDonService.InsertAsync(model);


            if (model.id > 0)
            {
                if (!await _mauHoaDonService.SaveSettingsToXsltAsync(model))
                    return this.BadRequest("Không thể lưu thiết lập vào file XSLT");

                var inserted = await _mauHoaDonService.SelectByIdAsync(model.id);
                if (inserted != null && inserted.xslt_path != model.xslt_path)
                {
                    inserted.xslt_path = model.xslt_path;
                    await _mauHoaDonService.UpdateAsync(inserted);
                }

                await this.SaveLogAsync($"Thêm mẫu hóa đơn: {model.name}", model);
                return this.OK(model);
            }
            return this.BadRequest();
        }
        /// <summary>
        /// Sửa mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPut]
        [MustAuthorized]
        public async Task<ContentResult> UpdateAsync([FromBody] mau_hoa_don model)
        {
            var user_id = this.GetUserId();
            var obj = await _mauHoaDonService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            if (obj.is_locked.HasValue && obj.is_locked.Value == true) return this.BadRequest("Mẫu hóa đơn đã khóa, không thể chỉnh sửa");

            //
            var mauHoaDonTemplate = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectVmByIdAsync(
                model.loai_hoa_don_ct_template_id > 0
                    ? model.loai_hoa_don_ct_template_id
                    : obj.loai_hoa_don_ct_template_id);
            var existingXsltPath = MauHoaDonService.ResolveContentPath(obj.xslt_path.ConvertToString());
            if (mauHoaDonTemplate != null
                && (obj.xslt_path.ConvertToString() == "" || !System.IO.File.Exists(existingXsltPath)))
            {
                var sourceFile = MauHoaDonService.ResolveContentPath(mauHoaDonTemplate.path);
                if (!System.IO.File.Exists(sourceFile))
                    return this.BadRequest("Không tìm thấy file template XSLT");

                var destFile = MauHoaDonService.BuildTemplateDestPath(obj.donvi_ma_dv.ConvertToString());
                var destinationDirectory = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
                System.IO.File.Copy(sourceFile, destFile, true);
                obj.xslt_path = MauHoaDonService.ToRelativeContentPath(destFile);
            }
            //

            obj.name = model.name;
            obj.so_qd = model.so_qd;
            obj.ngay_qd = model.ngay_qd?.Date ?? null;
            obj.logo_path = model.logo_path;
            obj.watermark_path = model.watermark_path;
            obj.vien_path = model.vien_path;
            obj.logo_position = model.logo_position;
            obj.is_show_wattermark_inner_table = model.is_show_wattermark_inner_table;
            obj.watermark_opacity = model.watermark_opacity;
            obj.advanced_settings_json = model.advanced_settings_json;
            obj.SetUpdateInfo(user_id);

            if (!await _mauHoaDonService.SaveSettingsToXsltAsync(obj))
                return this.BadRequest("Không thể lưu thiết lập vào file XSLT");

            var isUpdated = await _mauHoaDonService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"Cập nhật mẫu hóa đơn: {model.name}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        /// <summary>
        /// Active mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpPut]
        [Route("active")]
        [MustAuthorized]

        public async Task<ContentResult> UpdateActiveAsync([FromBody] MauHoaDonActiveRequest model)
        {
            var user_id = this.GetUserId();
            var obj = await _mauHoaDonService.SelectByIdAsync(model.id);
            if (obj == null) return this.BadRequest();
            if (obj.is_locked.HasValue && obj.is_locked.Value == true) return this.BadRequest("Mẫu hóa đơn đã khóa, không thể chỉnh sửa");
            var userInfo = this.GetUserInfo();
            var list = await _mauHoaDonService.SelectByDonViAsync(userInfo.donvi_ma_dv);
            var objLoaiHoaDonCTTemplate = await _serviceWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectVmByIdAsync(obj.loai_hoa_don_ct_template_id);
            if (model.is_active)
            {
                var checkOtherActive = list.Where(x => x.is_active == true &&
                objLoaiHoaDonCTTemplate != null &&
                x.loai_hoa_don_ct_id == objLoaiHoaDonCTTemplate.loai_hoa_don_ct_id &&
                x.id != obj.id).FirstOrDefault();
                if (checkOtherActive != null)
                {
                    return this.BadRequest("Chỉ được Sử dụng 1 mẫu trong mỗi loại hóa đơn. Vui lòng Ngưng sử dụng các mẫu hóa đơn khác cùng loại trước.");
                }
            }
            obj.is_active = model.is_active;
            obj.SetUpdateInfo(user_id);
            var isUpdated = await _mauHoaDonService.UpdateAsync(obj);
            if (isUpdated)
            {
                await this.SaveLogAsync($"{(obj.is_active ? "Sử dụng" : "Ngừng sử dụng")} mẫu hóa đơn: {obj.name}", model);
            }
            return isUpdated ? this.OK(obj) : this.BadRequest();
        }
        /// <summary>
        /// Xóa mẫu hóa đơn
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        [HttpDelete("{id}")]
        [MustAuthorized]
        public async Task<ContentResult> DeleteAsync([FromRoute] int id)
        {
            var obj = await _mauHoaDonService.SelectByIdAsync(id);
            if (obj == null) return this.BadRequest();
            //kiểm tra nếu đã phát hành thì không cho xóa
            if (obj.is_locked.HasValue && obj.is_locked.Value == true) return this.BadRequest("Mẫu hóa đơn đã khóa, không thể chỉnh sửa");
            var isDeleted = await _mauHoaDonService.DeleteAsync(obj.id);
            if (isDeleted) await this.SaveLogAsync($"Xóa mẫu hóa đơn: {obj.name}", null);
            return isDeleted ? this.OK(obj) : this.BadRequest();
        }

    }
}

