using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Contract.Service;
using Contracts.Service.HoaDon;
using Contracts.Service.Pdf;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;
using Model.Request.HoaDon;
using WebApp;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/hoa-don")]

    public class HoaDonTraCuuController : BaseController
    {
        private IHoaDonService _hoaDonService;
        private IPdfService _pdfService;
        public HoaDonTraCuuController(IServiceWrapper serviceWrapper, IPdfService pdfService) : base(serviceWrapper)
        {
            this._hoaDonService = _serviceWrapper.HoaDon.HoaDon;
            this._pdfService = pdfService;
        }

        [Route("ma-tra-cuu/{ma_tra_cuu}")]
        [HttpGet]
        public async Task<ContentResult> SelectByIdAsync(string ma_tra_cuu)
        {
            var model = await _hoaDonService.SelectHoaDonIdByMaTraCuuAsync(ma_tra_cuu);
            return this.OK(model);
        }
        [HttpGet("{id}/print")]
        // [MustAuthorized("[GET]api/hoa-don")]
        public async Task<ContentResult> GetHtmlPrintAsync([FromRoute] int id, [FromQuery] int page_size = 10, [FromQuery] string chuyen_doi = null)
        {
            // LogWriter.Writer($"Request {id} {page_size} {chuyen_doi}", "api/hoa-don/{id}/print", "");
            var result = await _hoaDonService.GetHtmlPrintAsync(
                id,
                page_size,
                chuyen_doi != null ? new MauHoaDonInChuyenDoiParam()
                {
                    nguoi_chuyen_doi = chuyen_doi
                } : null
            );
            // LogWriter.Writer($"GetHtmlPrintAsync Result {result.is_success}", "api/hoa-don/{id}/print", "");
            if (result.is_success)
            {
                var cacheKey = $"PRINT_HOA_DON_{id}_{page_size}_{chuyen_doi}";
                await _serviceWrapper.Cache.SetDataAsync<string>(cacheKey, result.data, DateTime.Now.AddHours(1));
            }
            return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpGet("{id}/html-bien-ban")]
        // [MustAuthorized("[GET]api/hoa-don")]
        public async Task<ContentResult> GetHtmlPrintBienBanAsync([FromRoute] int id)
        {
            var result = await _hoaDonService.GetHtmlPrintBienBanAsync(id);
            if (result.is_success)
            {
                return this.OK(result.data);
            }
            return this.BadRequest(result.message);
        }
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadPdf([FromRoute] int id, [FromQuery] int page_size = 10, [FromQuery] string chuyen_doi = null)
        {
            var html = "";
            // var cacheKey = $"PRINT_HOA_DON_{id}_{page_size}_{chuyen_doi}";
            // html = await _serviceWrapper.Cache.GetDataAsync<string>(cacheKey);
            // html = html.ConvertToString();
            // if (html == string.Empty)
            // {
            //     var result = await _hoaDonService.GetHtmlForDownloadAsync(
            //                    id,
            //                    page_size,
            //                    chuyen_doi != null ? new MauHoaDonInChuyenDoiParam()
            //                    {
            //                        nguoi_chuyen_doi = chuyen_doi
            //                    } : null
            //                );
            //     if (result.is_success)
            //     {
            //         html = result.data;
            //     }
            // }
            var result = await _hoaDonService.GetHtmlForDownloadAsync(
                              id,
                              page_size,
                              chuyen_doi != null ? new MauHoaDonInChuyenDoiParam()
                              {
                                  nguoi_chuyen_doi = chuyen_doi
                              } : null
                          );
            if (result.is_success)
            {
                html = result.data;
            }
            if (html == string.Empty) return null;
            var hoaDon = await _hoaDonService.SelectByIdAsync(id);
            if (hoaDon != null)
            {
                var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
                var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
                {
                    FileDownloadName = $"{hoaDon.nguoi_mua_mst}_{hoaDon.nguoi_mua_ten.ConvertToString()}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}_{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{hoaDon.ma_so_hoa_don.ConvertToString()}.pdf"
                };
                return fileContentResult;
            }
            return null;


            // return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpGet("{id}/pdf-bien-ban")]
        public async Task<IActionResult> DownloadPdfBienBan([FromRoute] int id, [FromQuery] int page_size = 10, [FromQuery] string chuyen_doi = null)
        {
            var html = "";
            var result = await _hoaDonService.GetHtmlPrintBienBanAsync(id);
            if (result.is_success)
            {
                html = result.data;
            }
            if (html == string.Empty) return null;
            var hoaDon = await _hoaDonService.SelectByIdAsync(id);
            if (hoaDon != null)
            {
                var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
                var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
                {
                    FileDownloadName = $"Bienban_{hoaDon.nguoi_mua_mst}_{hoaDon.nguoi_mua_ten.ConvertToString()}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}_{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{hoaDon.ma_so_hoa_don.ConvertToString()}.pdf"
                };
                return fileContentResult;
            }
            return null;
            // return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpPost("pdf/from-html")]
        public async Task<IActionResult> DownloadPdfFromHtmlAsync([FromBody] PrintPdfFromHtmlRequest printPdfFromHtmlRequest)
        {

            if (printPdfFromHtmlRequest.html != string.Empty)
            {
                var xmlBytes = await _pdfService.ConvertFromHtmlAsync(printPdfFromHtmlRequest.html);
                var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
                {
                    FileDownloadName = $"{printPdfFromHtmlRequest.file_name}.pdf"
                };
                return fileContentResult;
            }
            return null;


            // return result.is_success ? this.OK(result.data) : this.BadRequest(result.message);
        }
        [HttpGet]
        [Route("{id}/link/validate")]

        public async Task<ContentResult> ValidateHashAsync([FromRoute] int id, [FromQuery] string hash)
        {
            var isMatch = id.ToString().isMatch(hash);
            return isMatch ? this.OK() : this.BadRequest();
        }
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadXmlFile(int id)
        {
            var hoaDon = await _hoaDonService.SelectByIdAsync(id);
            if (hoaDon != null && hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP)
            {
                var xmlNhapResult = await _hoaDonService.CreateXmlKySoAsync(id, true);
                if (!xmlNhapResult.is_success) return this.BadRequest(xmlNhapResult.message);
                var xmlBytesNhap = Encoding.UTF8.GetBytes(xmlNhapResult.data);
                var resultNhap = new FileContentResult(xmlBytesNhap, "application/xml")
                {
                    FileDownloadName = $"{hoaDon?.nguoi_mua_mst ?? "}_{hoaDon?.hoa_don_dang_ky_phat_hanh_mau_so??"}_{hoaDon?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? ""}_{hoaDon?.ma_so_hoa_don.ToString() ?? ""}_nhap.xml"
                };

                return resultNhap;
            }
            // Đường dẫn đến tệp XML

            var xmlDataFile = (await _serviceWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(id)).Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP).LastOrDefault();

            if (hoaDon.hoa_don_hinh_thuc_code == "C")
            {
                xmlDataFile = (await _serviceWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(id)).Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.CO_QUAN_THUE_CHAP_NHAN && x.mltdiep == "202").LastOrDefault();
            }


            if (xmlDataFile == null || !System.IO.File.Exists(xmlDataFile.file_thong_diep_url))
            {
                return NotFound();
            }

            var xmlContent = await System.IO.File.ReadAllTextAsync(xmlDataFile.file_thong_diep_url, Encoding.UTF8);
            var xmlBytes = Encoding.UTF8.GetBytes(xmlContent);
            var result = new FileContentResult(xmlBytes, "application/xml")
            {
                FileDownloadName = $"{hoaDon?.nguoi_mua_mst ?? "}_{hoaDon?.hoa_don_dang_ky_phat_hanh_mau_so??"}_{hoaDon?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? ""}_{hoaDon?.ma_so_hoa_don.ToString() ?? ""}.xml"
            };

            return result;
        }
        [HttpGet("pdfs")]
        public async Task<IActionResult> DownloadPdfs([FromQuery] string hoaDonIds)
        {
            var ids = hoaDonIds.ConvertToList();
            if (ids.Count > 20) return this.BadRequest("Tối đa 20 hóa đơn 1 lần");
            var hoaDons = await _hoaDonService.SelectByIdsAsync(ids);

            var tasks = hoaDons.Select(async hoaDon =>
            {
                var htmlResult = await _hoaDonService.GetHtmlForDownloadAsync(hoaDon.id);
                var html = htmlResult.data;
                if (html == string.Empty) return null;
                var xmlBytes = await _pdfService.ConvertFromHtmlAsync(html);
                var fileContentResult = new FileContentResult(xmlBytes, "application/pdf")
                {
                    FileDownloadName = $"{hoaDon.nguoi_mua_mst}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}_{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{(hoaDon.ma_so_hoa_don.ConvertToInt() > 0 ? hoaDon.ma_so_hoa_don.ToString() : $"Nhap{hoaDon.id}")}.pdf"
                };
                return fileContentResult;
            }).ToList();
            await Task.WhenAll(tasks);
            var files = tasks.Where(x => x.Result != null).Select(x => x.Result).ToList();
            // Tạo file zip
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var file in files)
                {
                    var entry = zipArchive.CreateEntry(file.FileDownloadName, CompressionLevel.Fastest);
                    using var entryStream = entry.Open();
                    await entryStream.WriteAsync(file.FileContents, 0, file.FileContents.Length);
                }
            }

            // Đảm bảo Stream quay lại vị trí ban đầu để có thể đọc
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trả file zip về cho người dùng
            var zipFileName = "HoaDonPdf.zip";
            return new FileContentResult(memoryStream.ToArray(), "application/zip")
            {
                FileDownloadName = zipFileName
            };
        }
        [HttpGet("xmls")]
        public async Task<IActionResult> DownloadXmls([FromQuery] string hoaDonIds)
        {
            var ids = hoaDonIds.ConvertToList();
            var hoaDons = await _hoaDonService.SelectByIdsAsync(ids);
            var tasks = hoaDons.Select(async hoaDon =>
            {
                var xmlDataFile = (await _serviceWrapper.HoaDon.HoaDonLog.SelectByHoaDonAsync(hoaDon.id)).Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.GUI_THONG_DIEP).LastOrDefault();

                if (xmlDataFile == null || !System.IO.File.Exists(xmlDataFile.file_thong_diep_url))
                {
                    return null;
                }

                var xmlContent = await System.IO.File.ReadAllTextAsync(xmlDataFile.file_thong_diep_url, Encoding.UTF8);
                var xmlBytes = Encoding.UTF8.GetBytes(xmlContent);
                var result = new FileContentResult(xmlBytes, "application/xml")
                {
                    FileDownloadName = $"{hoaDon?.nguoi_mua_mst ?? "}_{hoaDon?.hoa_don_dang_ky_phat_hanh_mau_so??"}_{hoaDon?.hoa_don_dang_ky_phat_hanh_ky_hieu ?? ""}_{hoaDon?.ma_so_hoa_don.ToString() ?? ""}.xml"
                };
                return result;
            }).ToList();
            await Task.WhenAll(tasks);
            var files = tasks.Where(x => x.Result != null).Select(x => x.Result).ToList();
            // Tạo file zip
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var file in files)
                {
                    var entry = zipArchive.CreateEntry(file.FileDownloadName, CompressionLevel.Fastest);
                    using var entryStream = entry.Open();
                    await entryStream.WriteAsync(file.FileContents, 0, file.FileContents.Length);
                }
            }

            // Đảm bảo Stream quay lại vị trí ban đầu để có thể đọc
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Trả file zip về cho người dùng
            var zipFileName = "HoaDonXml.zip";
            return new FileContentResult(memoryStream.ToArray(), "application/zip")
            {
                FileDownloadName = zipFileName
            };
        }

        [HttpPost("pdfs-infor")]
        public async Task<IActionResult> GetInforPdfs([FromBody] HoaDonPdfInforRequest request)
        {
            var hoaDons = await _hoaDonService.SelectByMaSoHoaDonRangeAsync(
                request.donvi_ma_dv,
                request.ky_hieu,
                request.fromMaSo,
                request.toMaSo
            );

            if (hoaDons.Count() > 500) return this.BadRequest("Tối đa 500 hóa đơn 1 lần");

            var tasks = hoaDons.Select(async hoaDon =>
            {
                var htmlResult = await _hoaDonService.GetHtmlForDownloadAsync(hoaDon.id);
                var html = htmlResult.data;

                if (string.IsNullOrEmpty(html))
                {
                    hoaDon.file_name = null;
                    hoaDon.html = null;
                    return hoaDon;
                }

                var fileName = $"{hoaDon.nguoi_mua_mst}_{hoaDon.hoa_don_dang_ky_phat_hanh_mau_so}_{hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu}_{(hoaDon.ma_so_hoa_don.ConvertToInt() > 0 ? hoaDon.ma_so_hoa_don.ToString() : $"Nhap{hoaDon.id}")}.pdf";

                hoaDon.file_name = fileName;
                hoaDon.html = html;

                return hoaDon;
            }).ToList();

            var results = await Task.WhenAll(tasks);

            return Ok(results);
        }

    }



}

