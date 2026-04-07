using System.Diagnostics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Contracts.Service.RemoteSigning;
using Microsoft.Extensions.DependencyInjection;
using Model.Base;
using Model.RemoteSigning;
using Model.Respone.Upload;
using Model.Static;
using Service.Base;
using Service.Hub;
using WebApp;

namespace Service.RemoteSigning
{
    public class RemoteSigningSerivce : BaseService, IRemoteSigningSerivce
    {
        HoaDonPhatHanhHub _hoaDonPhatHanhHub;
        public RemoteSigningSerivce(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
        }

        public async Task<FunctionResult<string>> DangNhapAsync(DangNhapRequest request)
        {
            var maYeuCauRes = await this.GuiYeuCauKyAsync<DangNhapRequest>(request);
            if (maYeuCauRes.is_success)
            {
                // var result = await this.TryGetKetQuaKyThenClearAsync<string>(maYeuCauRes.data, "", new CancellationToken());
                // if (result)
                // {
                //     return new SuccessResult<string>("");
                // }
                var code = maYeuCauRes.data;
                var waitTime = AppSettings.FixedValue.RemoteSigningWaittimeSecond;
                var duraion = AppSettings.FixedValue.RemoteSigningDurationSecond;
                var stopwatch = Stopwatch.StartNew();
                while (stopwatch.Elapsed < TimeSpan.FromSeconds(duraion))
                {
                    try
                    {
                        var response = await this.GetKetQuaKyAsync(code);
                        if (response.is_success)
                        {
                            try
                            {
                                var cerInfo = this.ReadCerInfoAsync(response.data);
                                if (cerInfo != null)
                                {
                                    if (cerInfo.serial_number.ToUpper() == request.Serial.ToUpper())
                                    {
                                        await this.UpdateYeuCauKyThanhCongAsync(code);
                                        return new SuccessResult<string>(cerInfo.serial_number.ToUpper());
                                    }
                                    // xóa yêu cầu ký
                                    await this.DeleteYeuCauKyAsnc(code);
                                    return new ErrorResult<string>("");
                                }
                            }
                            catch (Exception ex)
                            {
                                return new ErrorResult<string>("");
                            }
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions as needed
                    }

                    await Task.Delay(TimeSpan.FromSeconds(waitTime));
                }
                // xóa yêu cầu ký
                await this.DeleteYeuCauKyAsnc(code);

            }
            return new ErrorResult<string>("");
        }

        public async Task<FunctionResult<string>> GetCertInfoAsync(int ma_but_ky)
        {
            try
            {
                var domain = AppSettings.FixedValue.RemoteSigningDomain;
                var url = $"{domain}/api/APISigncore/Getcert?idcts={ma_but_ky.ToString()}";
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent("", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        responseContent = responseContent.Replace("\\r", "").Replace("\\n", "").Replace("\"", "");
                        if (responseContent != string.Empty)
                            // var obj = responseContent.ConvertFromBase64<object>();
                            return new SuccessResult<string>(responseContent);
                    }
                    else
                    {


                    }
                }
                return new ErrorResult<string>("");
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        private async Task<FunctionResult<string>> GetKetQuaKyAsync(string code)
        {
            var domain = AppSettings.FixedValue.RemoteSigningDomain;
            var url = $"{domain}/api/APISigncore/Layketquaky?Code={code.ToString()}";
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    responseContent = responseContent.Replace("\"", "");
                    if (responseContent != "")
                    {
                        return new SuccessResult<string>(responseContent);
                        //    try
                        //    {
                        //      var check = this.ReadCerInfoAsync(responseContent);
                        //     return new SuccessResult<T>(check);
                        //    }
                        //    catch (System.Exception ex)
                        //    {
                        //      // TODO
                        //    }
                        //     var data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseContent);
                        //     return new SuccessResult<T>(data);
                    }

                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return new ErrorResult<string>("");
                }
            }
            return new ErrorResult<string>("");
        }
        private CerInfo ReadCerInfoAsync(string base64Cer)
        {
            try
            {
                // byte[] certBytes = Convert.FromBase64String(base64Cer);
                // X509Certificate2Collection certCollection = new X509Certificate2Collection();
                // certCollection.Import(certBytes); // Đọc toàn bộ chuỗi chứng thư

                // // Lấy chứng thư đầu tiên (leaf certificate)
                // X509Certificate2 certificate = certCollection[0];

                // // Xây dựng chuỗi chứng thư và xác minh thủ công
                // X509Chain chain = new X509Chain();
                // chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // Tắt kiểm tra thu hồi nếu không cần
                // foreach (var cert in certCollection)
                // {
                //     chain.ChainPolicy.ExtraStore.Add(cert); // Thêm tất cả chứng thư vào chuỗi
                // }

                // bool isValid = chain.Build(certificate);
                // if (!isValid)
                // {
                //     Console.WriteLine("Chuỗi chứng thư không hợp lệ:");
                //     foreach (var status in chain.ChainStatus)
                //     {
                //         Console.WriteLine(status.StatusInformation);
                //     }
                //     return null;
                // }
                // return new CerInfo()
                // {
                //     serial_number = certificate.SerialNumber,
                //     issuer = certificate.Issuer,
                //     not_after = certificate.NotAfter,
                //     not_before = certificate.NotBefore,
                //     public_key = certificate.PublicKey.Key.ToXmlString(false),
                //     signature_algorithm = certificate.SignatureAlgorithm.FriendlyName,
                //     version = certificate.Version.ToString(),
                //     // extensions = Newtonsoft.Json.JsonConvert.SerializeObject(certificate.Extensions.Select(x => x.Oid.FriendlyName)),
                //     subject = certificate.Subject,
                //     thumbprint = certificate.Thumbprint

                // };
                byte[] cerBytes = Convert.FromBase64String(base64Cer);
                X509Certificate2 cert = new X509Certificate2(cerBytes);
                return new CerInfo()
                {
                    serial_number = cert.SerialNumber,
                    issuer = cert.Issuer,
                    not_after = cert.NotAfter,
                    not_before = cert.NotBefore,
                    public_key = cert.PublicKey.Key.ToXmlString(false),
                    signature_algorithm = cert.SignatureAlgorithm.FriendlyName,
                    version = cert.Version.ToString(),
                    extensions = Newtonsoft.Json.JsonConvert.SerializeObject(cert.Extensions.Select(x => x.Oid.FriendlyName)),
                    subject = cert.Subject,
                    thumbprint = cert.Thumbprint

                };
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public async Task<FunctionResult<string>> GuiYeuCauKyAsync<T>(T request)
        {
            try
            {
                var domain = AppSettings.FixedValue.RemoteSigningDomain;
                var url = $"{domain}/api/APISigncore/Guiyeucauky_Mobilesign";
                var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);

                using (HttpClient client = new HttpClient())
                {

                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        responseContent = responseContent.Replace("\"", "");
                        // var code = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent);
                        return new SuccessResult<string>(responseContent);
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return new ErrorResult<string>("");


                    }
                }
            }
            catch (System.Exception ex)
            {
                return new ErrorResult<string>(ex.Message);
            }
        }

        public Task<FunctionResult<string>> KySoAsync(BaseRequest request)
        {
            return this.GuiYeuCauKyAsync<BaseRequest>(request);
        }

        public async Task<FunctionResult<string>> TryGetKetQuaKyThenClearAsync(string code, string user_id, CancellationToken cancellationToken)
        {

            var waitTime = AppSettings.FixedValue.RemoteSigningWaittimeSecond;
            var duraion = AppSettings.FixedValue.RemoteSigningDurationSecond;
            var stopwatch = Stopwatch.StartNew();
            while (!cancellationToken.IsCancellationRequested && stopwatch.Elapsed < TimeSpan.FromSeconds(duraion))
            {
                try
                {
                    var response = await this.GetKetQuaKyAsync(code);
                    if (response.is_success)
                    {
                        // update yêu cầu ký thành công
                        // push message kết quả;
                        await this.UpdateYeuCauKyThanhCongAsync(code);
                        await _hoaDonPhatHanhHub.OnRemoteSigningSuccess(new Model.Request.Hub.RemoteSigningSuccess()
                        {
                            request_code = code,
                            user_id = user_id
                        });
                        return new SuccessResult<string>(response.data);
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions as needed
                }

                await Task.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken);
            }
            // xóa yêu cầu ký
            await this.DeleteYeuCauKyAsnc(code);
            return new ErrorResult<string>("Hết hạn");
        }
        public async Task<bool> DeleteYeuCauKyAsnc(string code)
        {
            var domain = AppSettings.FixedValue.RemoteSigningDomain;
            var url = $"{domain}/api/APISigncore/Xoayeucauky_Code?Code={code}&phanloaiky=1";

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    responseContent = responseContent.Replace("\"", "");
                    return true;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return false;


                }
            }
        }
        public async Task<bool> UpdateYeuCauKyThanhCongAsync(string code)
        {
            var domain = AppSettings.FixedValue.RemoteSigningDomain;
            var url = $"{domain}/api/APISigncore/Capnhattrangthaihoantat_Code?code={code}&trangthai=1";

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    responseContent = responseContent.Replace("\"", "");
                    return true;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return false;


                }
            }
        }

        public async Task<FunctionResult<string>> DangNhapCodeAsync(DangNhapRequest request)
        {
            var maYeuCauRes = await this.GuiYeuCauKyAsync<DangNhapRequest>(request);
            if (maYeuCauRes.is_success)
            {

                var code = maYeuCauRes.data;
                return new SuccessResult<string>(code);
            }
            return new ErrorResult<string>();
        }

        public async Task<FunctionResult<string>> TryGetKetQuaKyThenClearAsync(string code)
        {
            var waitTime = AppSettings.FixedValue.RemoteSigningWaittimeSecond;
            var duraion = AppSettings.FixedValue.RemoteSigningDurationSecond;
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(duraion))
            {
                try
                {
                    LogWriter.Writer(code, "TryGetKetQuaKyThenClearAsync", "");
                    var response = await this.GetKetQuaKyAsync(code);
                    if (response.is_success)
                    {
                        // update yêu cầu ký thành công
                        // push message kết quả;
                        await this.UpdateYeuCauKyThanhCongAsync(code);
                        return new SuccessResult<string>(response.data);
                    }
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions as needed
                }

                await Task.Delay(TimeSpan.FromSeconds(waitTime));
            }
            // xóa yêu cầu ký
            await this.DeleteYeuCauKyAsnc(code);
            return new ErrorResult<string>("Hết hạn");
        }
    }
}