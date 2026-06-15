using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Xsl;
using Common;
using Contracts.Service.HoaDon;
using Contracts.Service.Pdf;
using Microsoft.Extensions.DependencyInjection;
using Model.Base;
using Model.Enum;
using Model.Request.ToKhai;
using Model.Static;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonSendEmailService : BaseService, IHoaDonSendEmailService
    {
       

        private IHoaDonLogService _hoaDonLogService;
        private IPdfService _pdfService;
        public HoaDonSendEmailService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._hoaDonLogService = _serviceWrapper.HoaDon.HoaDonLog;
            var scope = serviceProvider.CreateScope();
            _pdfService = scope.ServiceProvider.GetRequiredService<IPdfService>();

        }

        public async Task<FunctionResult<bool>> SendEmailHoaDonAsync(List<int> hoaDonIds, bool isCheckSendBienBan = false)
        {
            // LogWriter.Writer(Newtonsoft.Json.JsonConvert.SerializeObject(hoaDonIds), "SendEmailHoaDonAsync", "");

            var user = this.GetCurrentUser();
            var hoaDons = await _serviceWrapper.HoaDon.HoaDon.SelectByIdsAsync(hoaDonIds);
            var task = new List<Task>();
            foreach (var hoaDon in hoaDons)
            {
                if (hoaDon != null)
                {
                    var taskHoaDon = this.SendEmailAsync(hoaDon, hoaDon.nguoi_mua_email.ConvertToString(), user?.full_name ?? "", isCheckSendBienBan);
                    task.Add(taskHoaDon);
                }
                
            }
            await Task.WhenAll(task);
            return new SuccessResult<bool>(true);
        }

        public async Task<FunctionResult<bool>> SendEmailHoaDonAsync(HoaDonSendEmailCustomRequest request)
        {
            var user = this.GetCurrentUser();
            var hoaDon = await _serviceWrapper.HoaDon.HoaDon.SelectByIdAsync(request.id);
            if (hoaDon != null)
            {
                var result = await this.SendEmailAsync(hoaDon, request.emails, user?.full_name ?? "");
                if (result.is_success)
                {
                    return new SuccessResult<bool>();
                }
                else
                {
                    return new ErrorResult<bool>(result.message);
                }
            }
            return new ErrorResult<bool>("Dữ liệu không hợp lệ");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hoaDon"></param>
        /// <param name="email"></param>
        /// <param name="user_full_name"></param>
        /// <param name="isCheckSendBienBan">gui kem bien ban neu hoa don dieu chinh thay the</param>
        /// <returns></returns>
        private async Task<FunctionResult<bool>> SendEmailAsync(hoa_don hoaDon, string email, string user_full_name, bool isCheckSendBienBan = false)
        {
            var systemkey = hoaDon.nguoi_ban_mst + "_" + hoaDon.id + "_" + hoaDon.nguoi_mua_mst;
            var pdfBienBanUrl = "";
            if (isCheckSendBienBan && hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.DA_PHAT_HANH)
            {
                if (hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_THAY_THE ||
                         hoaDon.hoa_don_hinh_thuc_id == (int)e_hoa_don_hinh_thuc.HOA_DON_DIEU_CHINH
                         )
                {
                    var hoaDonLogs = await _hoaDonLogService.SelectByHoaDonAsync(hoaDon.id, (int)e_hoa_don_log_type.KY_SO_XML_BIEN_BAN_THANH_CONG);
                    var logBienBan = hoaDonLogs.Where(x => x.hoa_don_log_type_id == (int)e_hoa_don_log_type.TAO_XML_BIEN_BAN || x.hoa_don_log_type_id == (int)e_hoa_don_log_type.KY_SO_XML_BIEN_BAN_THANH_CONG).LastOrDefault();
                    if (logBienBan != null)
                    {
                        var xsltPath = "Template/bien-ban/bienbanDCTT.xslt";
                        var xsltArgument = new XsltArgumentList();
                        var xmlData = File.ReadAllText(logBienBan.file_thong_diep_url);
                        var htmlResult = await _serviceWrapper.Xslt.FillDataAsXmlAsync(xsltPath, xmlData, xsltArgument);
                        if (htmlResult.is_success)
                        {
                            var xmlBytes = await _pdfService.ConvertFromHtmlAsync(htmlResult.data);
                            var fileName = Guid.NewGuid().ToString() + ".pdf";
                            var filePath = $"Pdf/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{fileName}";
                            var directoryPath = Path.GetDirectoryName(filePath);
                            if (directoryPath != null && !Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            await File.WriteAllBytesAsync(filePath, xmlBytes);
                            pdfBienBanUrl = $"{AppSettings.FixedValue.FileDomain}/{filePath}";

                        }
                    }

                }
            }
            var subject = $"Hóa đơn xuất cho {hoaDon.nguoi_mua_mst}_ {hoaDon.nguoi_mua_ten_donvi} _ {hoaDon.ngay_hoa_don.ToString("dd/MM/yyyy")}";
            if (hoaDon.hoa_don_trang_thai_id == (int)e_hoa_don_trang_thai.NHAP)
            {
                subject = $"Thông tin hóa đơn nháp: {hoaDon.nguoi_mua_mst}_ {hoaDon.nguoi_mua_ten_donvi} _ {hoaDon.ngay_hoa_don.ToString("dd/MM/yyyy")}";

            }
            var donViBanHang = hoaDon.nguoi_ban_ten_donvi;
            var url = AppSettings.FixedValue.FileDomain + "/hoa-don/view/" + hoaDon.id + "?hash=" + hoaDon.id.ConvertToString().GenerateBcrypt();
            var body = "Kính gửi Quý khách hàng,";
            body += "<br /><br />Cảm ơn quý khách hàng đã mua hàng/sử dụng dịch vụ tại <b>" + donViBanHang + "</b>";
            body += "<br /> <b>" + donViBanHang + "</b> đã phát hành hóa đơn điện tử tới Quý khách. Quý khách Vui lòng nhấp vào liên kết sau để xem hóa đơn <br /> <a href='" + url + "' target='_blank'>" + url + "</a>";
            body += "<br />" + "<br />" + " hoặc tra cứu hóa đơn theo mã tra cứu: " + hoaDon.ma_tra_cuu + "<br /><br />";
            body += "Thông tin chi tiết của Hóa đơn: <br /><br />Mẫu số: " + hoaDon.hoa_don_dang_ky_phat_hanh_mau_so + "<br /> <br />Ký hiệu: " + hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu + "<br /><br />Số hóa đơn: " + hoaDon.ma_so_hoa_don;

            if (pdfBienBanUrl != "")
            {
                body += $"Số hóa đơn {hoaDon.so_hoa_don} là hóa đơn điều chỉnh/ thay thế cho hóa đơn số mẫu số {hoaDon.hoa_don_dang_ky_phat_hanh_mau_so_goc}, ký hiệu {hoaDon.hoa_don_dang_ky_phat_hanh_ky_hieu_goc}, số {hoaDon.ma_so_hoa_don_goc} ngày{(hoaDon.ngay_hoa_don_goc.HasValue ? hoaDon.ngay_hoa_don_goc.Value.ToString("dd/MM/yyyyy") : "")}";
                body += $"Vui lòng tải biên bản điều chỉnh/ thay thế theo link bên dưới:";
                body += $"<a href='{pdfBienBanUrl}' target='_blank'>{pdfBienBanUrl}</a>";
            }

            body += "<br /><br />Ghi chú: Hóa đơn điện tử có giá trị pháp lý tương đương với Hóa đơn giấy.<br /><br />";
            body += "Nếu có vấn đề gì cần hỗ trợ Quý khách vui lòng liên hệ: 1900545407" + "<br /><br />";
            body += "Đây là mail tự động, Quý khách vui lòng không trả lời lại mail này.<br /><br />";
            body += "<b><i>Trân trọng cám ơn sự hợp tác của Quý khách hàng!</i></b>";
            body += "<br/><br/><i>(Giải pháp Hoá đơn điện tử CA2-eInvoice được cung cấp bởi Công ty cổ phần công nghệ thẻ Nacencomm - 0103930279)</i><br/>";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            SendMailapi(systemkey, email, "", subject, body, "CA2 EINVOICE");

            return await _serviceWrapper.Core.Email.SendEmailAsync(new Model.Request.Email.SendEmailRequest()
            {
                Body = body,
                EmailAddress = email.Split(";").Select(x => x.Trim()).Where(x => x != string.Empty).ToList(),
                isHtml = true,
                SendByUser = user_full_name,
                Subject = subject,
            });
        }

        public  string SendMailapi(string systemKey, string to, string cc, string subject, string bodyHtml, string displayName)
        {
          HttpClient _httpClient;
          string taikhoan = "0103930279WEBHN";
          string matkhau = "2026CA2WEBHN920eab4b969a4afda9fae0da7469d668";
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
            _httpClient.BaseAddress = new Uri("http://118.71.99.155:8001/");
            try
            {
                var payload = new
                {
                    taikhoan,
                    matkhau,
                    SystemKey = systemKey,
                    To = to,
                    Cc = cc,
                    Subject = subject,
                    BodyHtml = bodyHtml,
                    DisplayName = displayName
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient
                    .PostAsync("api/SendMailCA2/SendMail", content)
                    .GetAwaiter()
                    .GetResult();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}