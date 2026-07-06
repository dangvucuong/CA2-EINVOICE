using System.Xml.Xsl;
using System.Text.RegularExpressions;
using Common;
using Contracts.Service.HoaDon;
using Model.Request.HoaDon;
using Model.Respone.HoaDon;
using Model.Table;
using Service.Base;
using WebApp;

namespace Service.HoaDon
{
    public class LoaiHoaDonCTTemplateService : CRUDServiceWithCache<loai_hoa_don_ct_template>, ILoaiHoaDonCTTemplateService
    {
        public LoaiHoaDonCTTemplateService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.LoaiHoaDonCTTemplate;
        }

        public Task<loai_hoa_don_ct_template_vm> SelectVmByIdAsync(int id)
        {
            return _repositoryWrapper.HoaDon.LoaiHoaDonCTTemplate.SelectVmByIdAsync(id);
        }

        public async Task<string> GenerateHtmlAsync<T>(string filePath, T data)
        {

            var result = await _serviceWrapper.Xslt.FillDataAsync(filePath, data);
            if (result.is_success)
            {
                return result.data;
            }
            return string.Empty;
        }

        public async Task<string> GenerateHtmlAsync<T>(int loai_hoa_don_ct_template_id, T data)
        {
            var tempalte = await this.SelectVmByIdAsync(loai_hoa_don_ct_template_id);
            if (tempalte != null && tempalte.path.ConvertToString() != string.Empty)
            {
                return await this.GenerateHtmlAsync(tempalte.path, data);
            }
            return string.Empty;
        }
        public async Task<string> GeneratePrintHtmlAsync(int loai_hoa_don_ct_template_id, MauHoaDonCreateHtmlInput input)
        {
            var html = await this.GenerateHtmlAsync(loai_hoa_don_ct_template_id, input.hoa_don);
            return html;
        }

        public async Task<string> GeneratePreviewAsync(mau_hoa_don mauHoaDon)
        {
            var inputData = await _serviceWrapper.HoaDon.MauHoaDon.CreateSampleData(mauHoaDon);
            var xml = inputData.ConvertToXml();
            var html = "";
            if (mauHoaDon.id <= 0 || mauHoaDon.xslt_path.ConvertToString() == "")
            {
                html = await this.GenerateHtmlAsync(mauHoaDon.loai_hoa_don_ct_template_id, inputData);
            }
            else
            {
                html = await this.GenerateHtmlAsync(mauHoaDon.xslt_path, inputData);
            }

            var bgb64 = "";
            var showInnerTable = mauHoaDon.is_show_wattermark_inner_table == true;
            var bgstyle = "width:900px;margin:auto; border:2px solid black; padding-top:20px;z-index:1;position: relative;";
            if (showInnerTable)
            {
                bgstyle += "background-image: url(''); background-size:80%; background-position: center;background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;background-repeat:no-repeat";
            }
            else
            {
                bgstyle += "background-image: url('{paramWaterMark}'); background-size:80%; background-position: center;background-color: hsla(0,0%,100%,paramOpacity;);background-blend-mode: overlay;background-repeat:no-repeat";
            }
            var noidungdisabled = "&#160;";
            var styledisabled = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:none;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            var stylemau = "position:absolute;z-index:0;width:300px;height:140px;border:5px solid red;background:transparent;display:block;top:45%;left:40%;color:red;font-size:70pt;text-align:center;padding-top:10px;";
            var paramsubtitle = "none";
            var paramSubtitleDiv = "none";
            var paramsubtitlecontent = String.Empty;
            var paramSubtitleContentDiv = "&#160;";


            html = html.Replace("viewstyle", bgstyle)
            .Replace("paramLogo", "{paramLogo}")
           .Replace("paramVien", "{paramVien}")
           .Replace("paramChuyendoi", "display:normal")
           .Replace("paramSign", "display:normal")
           .Replace("paramMau", stylemau)
           .Replace("paramNguoiCD", "width:100%;text-align:center;display:normal")
           .Replace("paramdisable", styledisabled).Replace("contentDisable", noidungdisabled)
           .Replace("param1_1", paramsubtitle)
           .Replace("param1", paramsubtitlecontent)
           .Replace("param2_2", paramSubtitleDiv)
           .Replace("param2", paramSubtitleContentDiv)
           .Replace("paramdisable", styledisabled)
           .Replace("contentDisable", noidungdisabled)
           .Replace("paramlien", "0").Replace("paramdisplay", "display:none");

            var watermarkUrl = mauHoaDon.watermark_path?.ConvertToString().Replace('\\', '/') ?? "";
            var paramOpacity = (1 - ((mauHoaDon.watermark_opacity ?? 50) * 1.0 / 100))
                .ConvertToDouble(2)
                .ToString()
                .Replace(",", ".");

            if (showInnerTable)
            {
                html = html.Replace("{paramWaterMark}", "");
                if (!string.IsNullOrEmpty(watermarkUrl))
                {
                    html = html.Replace("paramWaterMarkTable;", watermarkUrl);
                    html = html.Replace(
                        "paramTableBG",
                        $"background-image:url('{watermarkUrl}');background-size:cover;background-position:center;background-repeat:no-repeat;background-color:hsla(0,0%,100%,{paramOpacity});background-blend-mode:overlay;");
                }
            }
            else if (!string.IsNullOrEmpty(watermarkUrl))
            {
                html = html.Replace("{paramWaterMark}", watermarkUrl);
                html = html.Replace("paramWaterMarkTable;", "");
                html = html.Replace("paramTableBG", "");
            }

            html = html.Replace("paramOpacity;", paramOpacity);
            html = html.Replace("paramOpacity;", paramOpacity);

            if (showInnerTable && !string.IsNullOrEmpty(watermarkUrl))
            {
                const string innerTableCss =
                    "<style>table.inner-watermark-table td,table.inner-watermark-table th,table[style*=\"background-image\"] td,table[style*=\"background-image\"] th{background-color:transparent !important;}</style>";
                if (!html.Contains("inner-watermark-table") && html.Contains("</head>"))
                {
                    html = html.Replace("</head>", innerTableCss + "</head>");
                }

                html = Regex.Replace(
                    html,
                    @"<div style=""background:url\('([^']*)'\);background-color:\s*hsla\(0,0%,100%,([^)]+)\);background-blend-mode:\s*overlay;"">\s*<table style=""",
                    m =>
                        $"<div><table class=\"inner-watermark-table\" style=\"background-image:url('{m.Groups[1].Value}');background-size:cover;background-position:center;background-repeat:no-repeat;background-color:hsla(0,0%,100%,{m.Groups[2].Value});background-blend-mode:overlay;",
                    RegexOptions.IgnoreCase);
            }

            return html;
        }


        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "loai_hoa_don_ct_template:";
        }

        public async Task<string> GeneratePrintHtmlAsync(int loai_hoa_don_ct_template_id, MauHoaDonCreateHtmlInput input, XsltArgumentList xsltArgumentList)
        {
            var tempalte = await this.SelectVmByIdAsync(loai_hoa_don_ct_template_id);
            if (tempalte != null && tempalte.path.ConvertToString() != string.Empty)
            {
                var result = await _serviceWrapper.Xslt.FillDataAsync(tempalte.path, input, xsltArgumentList);
                if (result.is_success)
                {
                    return result.data;
                }
            }

            return string.Empty;
        }

        public async Task<string> GeneratePrintHtmlAsync(int loai_hoa_don_ct_template_id, string xmlData, XsltArgumentList xsltArgumentList)
        {
            var tempalte = await this.SelectVmByIdAsync(loai_hoa_don_ct_template_id);
            if (tempalte != null && tempalte.path.ConvertToString() != string.Empty)
            {
                var result = await _serviceWrapper.Xslt.FillDataAsXmlAsync(tempalte.path, xmlData, xsltArgumentList);
                if (result.is_success)
                {
                    var input = result.data;
                    string searchString = "<html"; //
                    int index = result.data.IndexOf(searchString);
                    if (index != -1)
                    {
                        return input.Substring(index);
                    }
                    return result.data;
                }
            }

            return string.Empty;
        }

        public async Task<string> GeneratePrintHtmlAsync(mau_hoa_don mauHoaDon, MauHoaDonCreateHtmlInput input, XsltArgumentList xsltArgumentList)
        {
            LogWriter.Writer($"GeneratePrintHtmlAsync", "Start", "");
            if (mauHoaDon != null && mauHoaDon.xslt_path.ConvertToString() != string.Empty)
            {

                var result = await _serviceWrapper.Xslt.FillDataAsync(mauHoaDon.xslt_path.ConvertToString(), input, xsltArgumentList);
                if (result.is_success)
                {
                    return result.data;
                }
            }

            return string.Empty;
        }

        public async Task<string> GeneratePrintHtmlAsyncv1(mau_hoa_don mauHoaDon, string xmlData, XsltArgumentList xsltArgumentList)
        {
            // var tempalte = await this.SelectByIdAsync(loai_hoa_don_ct_template_id);
            if (mauHoaDon != null && mauHoaDon.xslt_path.ConvertToString() != string.Empty)
            {
                var result = await _serviceWrapper.Xslt.FillDataAsXmlAsyncV1(mauHoaDon.xslt_path.ConvertToString(), xmlData, xsltArgumentList);
                if (result.is_success)
                {
                    var input = result.data;

                    int startIndex = input.IndexOf("<html", StringComparison.OrdinalIgnoreCase);
                    int endIndex = input.IndexOf("</html>", StringComparison.OrdinalIgnoreCase);

                    if (startIndex != -1 && endIndex != -1)
                    {
                        endIndex += "</html>".Length;
                        return input.Substring(startIndex, endIndex - startIndex);
                    }

                    return input;
                }
            }

            return string.Empty;
        }

        public async Task<string> GeneratePrintHtmlAsync(mau_hoa_don mauHoaDon, string xmlData, XsltArgumentList xsltArgumentList)
        {
            // var tempalte = await this.SelectByIdAsync(loai_hoa_don_ct_template_id);
            if (mauHoaDon != null && mauHoaDon.xslt_path.ConvertToString() != string.Empty)
            {
                var result = await _serviceWrapper.Xslt.FillDataAsXmlAsync(mauHoaDon.xslt_path.ConvertToString(), xmlData, xsltArgumentList);
                if (result.is_success)
                {
                    var input = result.data;

                    int startIndex = input.IndexOf("<html", StringComparison.OrdinalIgnoreCase);
                    int endIndex = input.IndexOf("</html>", StringComparison.OrdinalIgnoreCase);

                    if (startIndex != -1 && endIndex != -1)
                    {
                        endIndex += "</html>".Length;
                        return input.Substring(startIndex, endIndex - startIndex);
                    }

                    return input;
                }
            }

            return string.Empty;
        }

        public async Task<string> GeneratePrintHtmlFromXsltContentAsync(string xsltContent, string xmlData, XsltArgumentList xsltArgumentList)
        {
            var result = await _serviceWrapper.Xslt.FillDataAsXmlFromXsltContentAsync(xsltContent, xmlData, xsltArgumentList);
            if (result.is_success)
            {
                var input = result.data;
                string searchString = "<html"; //
                int index = result.data.IndexOf(searchString);
                if (index != -1)
                {
                    return input.Substring(index);
                }
                return result.data;
            }
            return string.Empty;
        }


        public async Task<string> GeneratePrintHtmlFromXsltContentAsyncV1(string xsltContent, string xmlData, XsltArgumentList xsltArgumentList)
        {
            var result = await _serviceWrapper.Xslt.FillDataAsXmlFromXsltContentAsyncV1(xsltContent, xmlData, xsltArgumentList);
            if (result.is_success)
            {
                var input = result.data;
                string searchString = "<html"; //
                int index = result.data.IndexOf(searchString);
                if (index != -1)
                {
                    return input.Substring(index);
                }
                return result.data;
            }
            return string.Empty;
        }
    }
}