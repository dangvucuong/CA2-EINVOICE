using System.Net;
using System.Text;
using Common;
using Contract.Service.Core;
using Microsoft.Extensions.DependencyInjection;
using Model.Base;
using Model.Request.Account;
using Model.Request.Email;
using Model.Respone.Account;
using Model.Static;
using Model.Table;
using Newtonsoft.Json.Linq;
using Service.Base;
using Service.Hub;
using WebApp;

namespace Service.Core
{
    public class AccountService : BaseService, IAccountService
    {
        HoaDonPhatHanhHub _hoaDonPhatHanhHub;
        public AccountService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._hoaDonPhatHanhHub = _serviceProvider.GetRequiredService<HoaDonPhatHanhHub>();
        }


        private List<menu> SelectAccessAll(List<menu> menu_alls, List<menu> menu_added)
        {
            var result = new List<menu>();
            foreach (var item in menu_added)
            {
                result.Add(item);
                var parent = menu_alls.Where(x => x.id == item.menu_id_parent).FirstOrDefault();
                if (parent != null)
                {
                    AddParent(result, menu_alls, parent);
                }
            }
            return result;
        }
        private void AddParent(List<menu> menu_all_parents, List<menu> menu_alls, menu parent)
        {
            if (menu_all_parents.Where(x => x.id == parent.id).Count() <= 0)
            {
                menu_all_parents.Add(parent);
                var g_parent = menu_alls.Where(x => x.id == parent.menu_id_parent).FirstOrDefault();
                if (g_parent != null)
                {
                    AddParent(menu_all_parents, menu_alls, g_parent);
                }
            }

        }
        private void AddChildNode(MenuItemRespone parent, int menu_parent_id, List<menu> menuAll)
        {
            var childs = menuAll.Where(x => x.menu_id_parent == menu_parent_id).OrderBy(x => x.sort_idx).ThenBy(x => x.name).ToList();
            if (childs.Count > 0)
            {
                parent.items = new List<MenuItemRespone>();
                foreach (var child in childs)
                {
                    var item = new MenuItemRespone()
                    {
                        id = child.id,
                        items = new List<MenuItemRespone>(),
                        icon = child.icon,
                        path = child.path,
                        name = child.name,
                        name_en = child.name_en,
                        menu_id = child.id,
                        sub_system_id = child.sub_system_id
                    };
                    parent.items.Add(item);
                    AddChildNode(item, child.id, menuAll);
                }
            }
        }
        private async Task<List<MenuItemRespone>> SelectByUserAsync(int sys_user_id, int sub_system_id)
        {
            var taskMenuAll = _repositoryWrapper.User.Menu.SelectAllAsync();
            var taskMenuUser = _repositoryWrapper.User.Menu.SelectByUserAsync(sys_user_id, sub_system_id);
            await Task.WhenAll(taskMenuAll, taskMenuUser);
            var menuAll = taskMenuAll.Result.ToList();
            var menuUser = taskMenuUser.Result.ToList();
            // var menuAll = await _repositoryWrapper.User.Menu.SelectAllAsync();
            // var menuUser = await _repositoryWrapper.User.Menu.SelectByUserAsync(sys_user_id, sub_system_id);
            var menu_all_parents = SelectAccessAll(menuAll.ToList(), menuUser.ToList());
            var source = new List<MenuItemRespone>();
            var roots = menu_all_parents.Where(x => x.menu_id_parent == 0).OrderBy(x => x.sort_idx).ThenBy(x => x.name).ToList();
            foreach (var root in roots)
            {
                var item = new MenuItemRespone()
                {
                    icon = root.icon,
                    id = root.id,
                    items = new List<MenuItemRespone>(),
                    path = root.path,
                    name = root.name,
                    name_en = root.name_en,
                    menu_id = root.id,
                    sub_system_id = root.sub_system_id
                };
                source.Add(item);
                AddChildNode(item, root.id, menu_all_parents);
            }
            return source;
        }

        public async Task<ProfileRespone?> GetProfileAsync(int user_id, int sub_system_id = 0)
        {
            var taskUser = _serviceWrapper.User.User.SelectByIdAsync(user_id);
            var taskRole = _repositoryWrapper.User.Role.SelectByUserAsync(user_id);
            var taskApi = _repositoryWrapper.User.Api.SelectByUserAsync(user_id);
            var taskMenu = this.SelectByUserAsync(user_id, sub_system_id);
            await Task.WhenAll(taskUser, taskRole, taskApi, taskMenu);
            var user = taskUser.Result;
            if (user == null) return null;

            // var roles = await _repositoryWrapper.User.Role.SelectByUserAsync(user_id);
            // var apis = await _repositoryWrapper.User.Api.SelectByUserAsync(user_id);
            // var menus = await this.SelectByUserAsync(user_id, sub_system_id);
            var roles = taskRole.Result.ToList();
            var apis = taskApi.Result.ToList();
            var menus = taskMenu.Result.ToList();
            var don_vi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(user.donvi_ma_dv);
            var result = new ProfileRespone()
            {
                email = user.email,
                full_name = user.full_name,
                username = user.username,
                user_id = user.id,
                roles = roles.ToList(),
                apis = apis.ToList(),
                menus = menus.ToList(),
                donvi_ma_dv = user.donvi_ma_dv,
                serial_number = user.serial_number,
                donvi = don_vi,
                is_serial_remote_signing_verified = user.is_serial_remote_signing_verified,
                serial_remote_signing_numner = user.serial_remote_signing_numner,
                vender_id = user.vender_id.ConvertToString(),
                is_hsm_signing = user.is_hsm_signing,
                is_remote_signing = user.rs_ma_but_ky.ConvertToString() != ""
            };
            await _serviceWrapper.Cache.SetDataAsync<ProfileRespone>("TOKEN_PROFILE_" + user.id.ToString(), result, null);
            return result;
        }

        public async Task<TokenInfo?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var refreshTokenInfo = _serviceWrapper.Core.JwtToken.ReadRefreshToken(request.refresh_token);
            if (refreshTokenInfo.is_success)//&& refreshTokenInfo.data.access_token == request.access_token)
            {
                var profile = await this.GetProfileAsync(refreshTokenInfo.data.user_id);
                if (profile == null) return null;
                var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
                {
                    full_name = profile.full_name,
                    id = profile.user_id,
                    username = profile.username,
                    donvi_ma_dv = profile.donvi_ma_dv,
                    vender_id = profile.vender_id,
                    is_hsm_signing = profile.is_hsm_signing,
                    is_remote_signing = profile.is_remote_signing,
                });
                var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
                {
                    access_token = accessToken,
                    user_id = profile.user_id
                });
                return new TokenInfo()
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                };
            }
            return null;
        }
        private async Task<FunctionResult<LoginRespone>> LoginWithPasskeyAsync(LoginRequest request)
        {

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    accessToken = request.password,
                    privateKey = "ca2einv_MyHLaQBxhn5eX2KMpD3JP6TuNFdjZr7kzCMC"
                };
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://api.nacecomm.online/webauthn/verify-access-token", content);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var verifyResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PasskeyRespone>(responseContent);
                    var email = verifyResponse.user_name;
                    // Tách chuỗi bằng ký tự '_'
                    string[] parts = email.Split(new[] { '_' }, 2);

                    // Lấy giá trị trước và sau ký tự '_'
                    string donvi_ma_dv = parts.Length > 0 ? parts[0] : string.Empty;
                    string username = parts.Length > 1 ? parts[1] : string.Empty;
                    var user = await _serviceWrapper.User.User.SelectByUsernameAsync(donvi_ma_dv, username);
                    if (user == null) return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");

                    var profile = await this.GetProfileAsync(user.id);
                    if (profile == null) return new ErrorResult<LoginRespone>("Đăng nhập thất bại");
                    var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
                    {
                        full_name = profile.full_name,
                        id = profile.user_id,
                        username = profile.username,
                        donvi_ma_dv = profile.donvi_ma_dv,
                        vender_id = profile.vender_id,
                        is_hsm_signing = profile.is_hsm_signing,
                        is_remote_signing = profile.is_remote_signing,
                    });
                    var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
                    {
                        access_token = accessToken,
                        user_id = profile.user_id
                    });
                    return new SuccessResult<LoginRespone>(data: new LoginRespone()
                    {
                        profile = profile,
                        token_info = new TokenInfo()
                        {
                            access_token = accessToken,
                            refresh_token = refreshToken
                        }
                    });
                }

            }
            return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");

        }
        public async Task<FunctionResult<LoginRespone>> LoginAsync(LoginRequest request)
        {
            if (request.donvi_ma_dv == "passkey" && request.username == "passkey")
            {
                return await this.LoginWithPasskeyAsync(request);
            }
            var user = await _serviceWrapper.User.User.SelectByUsernameAsync(request.donvi_ma_dv, request.username);
            if (user == null) return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");
            var isValid = false;
            if (request.password.ConvertToString().isMatch(user.password)) isValid = true;
            if (!isValid)
            {
                if (AppSettings.FixedValue.DevPassword != string.Empty &&
                request.password == AppSettings.FixedValue.DevPassword)
                {
                    isValid = true;
                }
            }
            if (!isValid) return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");
            var profile = await this.GetProfileAsync(user.id);
            if (profile == null) return new ErrorResult<LoginRespone>("Đăng nhập thất bại");
            var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
            {
                full_name = profile.full_name,
                id = profile.user_id,
                username = profile.username,
                donvi_ma_dv = profile.donvi_ma_dv,
                vender_id = profile.vender_id,
                is_hsm_signing = profile.is_hsm_signing,
                is_remote_signing = profile.is_remote_signing,
            });
            var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
            {
                access_token = accessToken,
                user_id = profile.user_id
            });
            return new SuccessResult<LoginRespone>(data: new LoginRespone()
            {
                profile = profile,
                token_info = new TokenInfo()
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                }
            });

        }

        public async Task<FunctionResult<SendOTPRespone>> SendOTPForgetPWAsync(ForgetPasswordSendOTPRequest request)
        {
            if (request.donvi_ma_dv.ConvertToString() == "" || request.email.ConvertToString() == "")
                return new ErrorResult<SendOTPRespone>("Tài khoản không hợp lệ");
            var user = await _serviceWrapper.User.User.SelectByEmailAsync(request.donvi_ma_dv, request.email);
            if (user == null) return new ErrorResult<SendOTPRespone>("Tài khoản không hợp lệ");
            var appOTPTsodaySended = await _serviceWrapper.User.OTP.SelectByPhoneNumberOrEmailAsync(request.donvi_ma_dv, request.email, DateTime.Now);
            if (appOTPTsodaySended.Count() >= AppSettings.FixedValue.LimitOTPPerDay)
                return new ErrorResult<SendOTPRespone>("Đã vượt giới hạn OTP trong một ngày");
            var appOTP = await this.SendOTPValidEmailAsync(request.email);
            if (appOTP == null) return new ErrorResult<SendOTPRespone>("Gửi OTP thất bại");
            return new SuccessResult<SendOTPRespone>("Success", new SendOTPRespone()
            {
                email = request.email,
                expire_at = appOTP.expire_at
            });
        }

        public async Task<FunctionResult<bool>> ResetNewPWAsync(ResetNewPassWordRequest request)
        {
            if (request.donvi_ma_dv.ConvertToString() == "" || request.email.ConvertToString() == "")
                return new ErrorResult<bool>("Tài khoản không hợp lệ");
            var user = await _serviceWrapper.User.User.SelectByEmailAsync(request.donvi_ma_dv, request.email);
            if (user == null) return new ErrorResult<bool>("Tài khoản không hợp lệ");
            var appOTP = await _serviceWrapper.User.OTP.SelectByPhoneNumberOrEmailAsync(request.donvi_ma_dv, request.email);
            if (appOTP == null || appOTP.expire_at <= DateTime.Now) return new ErrorResult<bool>("OTP không hợp lệ");
            Random rnd = new Random();
            var newPW = rnd.Next(1, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString();
            var isUpdated = await this.ChangePWAsync(user.id, newPW);
            if (!isUpdated) return new ErrorResult<bool>("Đổi mật khẩu thất bại");
            //send email new PW
            var body = "";
            body += $"<p>Mật khẩu mới của bạn là: {newPW}</p>";
            var result = await _serviceWrapper.Core.Email.SendEmailAsync(new SendEmailRequest()
            {
                EmailAddress = new List<string>() { request.email },
                Body = body,
                isHtml = true,
                SendByUser = "",
                Subject = "Khôi phục mật khẩu"
            });
            return new SuccessResult<bool>();

        }
        public async Task<app_otp> SendOTPValidEmailAsync(string email)
        {

            Random rnd = new Random();
            var OTP = rnd.Next(1, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString()
            + rnd.Next(0, 10).ToString();
            var otpObj = new app_otp()
            {
                otp = OTP,
                expire_at = DateTime.Now.AddMinutes(5),
                phone_number = string.Empty,
                email = email
            };
            otpObj.SetInsertInfo(0);
            otpObj.id = await _repositoryWrapper.User.OTP.InsertAsync(otpObj);
            if (otpObj.id > 0)
            {
                var body = "";
                body += $"<p>Mã xác thực của bạn là: {OTP}</p>";
                var result = await _serviceWrapper.Core.Email.SendEmailAsync(new SendEmailRequest()
                {
                    EmailAddress = new List<string>() { email },
                    Body = body,
                    isHtml = true,
                    SendByUser = "",
                    Subject = "Xác Thực Hòm Thư Điện Tử"
                });

                if (result.is_success)
                {
                    return otpObj;
                }
                return null;
            }
            return null;


        }

        public async Task<bool> ChangePWAsync(int user_id, string newPW)
        {
            return await _repositoryWrapper.User.User.ChangePWAsync(user_id, newPW.GenerateBcrypt());
        }

        public async Task<FunctionResult<LoginRespone>> LoginAsync(LoginSerialRequest request)
        {
            var user = await _serviceWrapper.User.User.SelectBySerialAsync(request.serial, request.mst);
            if (user == null) return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");
            //var isValid = false;
            //if (request.serial.ConvertToString()==user.serial_number) isValid = true;
            //if (!isValid) return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");
            var profile = await this.GetProfileAsync(user.id);
            if (profile == null) return new ErrorResult<LoginRespone>("Đăng nhập thất bại");
            var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
            {
                full_name = profile.full_name,
                id = profile.user_id,
                username = profile.username,
                donvi_ma_dv = profile.donvi_ma_dv,
                vender_id = profile.vender_id,
                is_hsm_signing = profile.is_hsm_signing,
                is_remote_signing = profile.is_remote_signing,
            });
            var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
            {
                access_token = accessToken,
                user_id = profile.user_id
            });
            return new SuccessResult<LoginRespone>(data: new LoginRespone()
            {
                is_verify_cert = true,
                profile = profile,
                token_info = new TokenInfo()
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                }
            });
        }

        public async Task<FunctionResult<bool>> ChangePWAsync(ChangePassWordRequest request)
        {
            var userId = this.GetCurrentUserId();
            var account = await _serviceWrapper.User.User.SelectByIdAsync(userId);
            if (account != null)
            {
                if (request.old_password.isMatch(account.password))
                {
                    await _repositoryWrapper.User.User.ChangePWAsync(account.id, request.new_password.GenerateBcrypt());
                    return new SuccessResult<bool>();
                }
            }
            return new ErrorResult<bool>();

        }

        public async Task<FunctionResult<LoginRespone>> LoginAsync(LoginRSRequest request)
        {
            var user = await _serviceWrapper.User.User.SelectByMaButKyAsync(request.rs_ma_but_ky);
            if (user == null) return new ErrorResult<LoginRespone>("Thông tin đăng nhập không hợp lệ");
            var result = await _serviceWrapper.RemoteSigningSerivce.DangNhapAsync(new Model.RemoteSigning.DangNhapRequest(
                Serial: user.serial_number,
                Masothue: user.donvi_ma_dv,
                Mahethong: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Email: user.email.ConvertToString()
            ));
            if (result.is_success)
            {
                var profile = await this.GetProfileAsync(user.id);
                if (profile == null) return new ErrorResult<LoginRespone>("Đăng nhập thất bại");
                var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
                {
                    full_name = profile.full_name,
                    id = profile.user_id,
                    username = profile.username,
                    donvi_ma_dv = profile.donvi_ma_dv,
                    vender_id = profile.vender_id,
                    is_hsm_signing = profile.is_hsm_signing,
                    is_remote_signing = profile.is_remote_signing,
                });
                var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
                {
                    access_token = accessToken,
                    user_id = profile.user_id
                });
                return new SuccessResult<LoginRespone>(data: new LoginRespone()
                {
                    is_verify_cert = true,
                    profile = profile,
                    token_info = new TokenInfo()
                    {
                        access_token = accessToken,
                        refresh_token = refreshToken
                    }
                });
            }
            return new ErrorResult<LoginRespone>("Xác thực thất bại");

        }

        public async Task<FunctionResult<bool>> DeletePasskeyAsync(LoginRequest request)
        {
            var user = await _serviceWrapper.User.User.SelectByUsernameAsync(request.donvi_ma_dv, request.username);
            if (user == null) return new ErrorResult<bool>("Thông tin đăng nhập không hợp lệ");
            var isValid = false;
            if (request.password.ConvertToString().isMatch(user.password)) isValid = true;
            if (!isValid)
            {
                if (AppSettings.FixedValue.DevPassword != string.Empty &&
                request.password == AppSettings.FixedValue.DevPassword)
                {
                    isValid = true;
                }
            }
            if (!isValid) return new ErrorResult<bool>("Thông tin đăng nhập không hợp lệ");
            using (var client = new HttpClient())
            {
                var payload = new
                {
                    mabutky = "12345",
                    username = $"{request.donvi_ma_dv}_{request.username}",
                    domainName = AppSettings.FixedValue.RegistedDomainPasskey,
                    privateKey = "ca2einv_MyHLaQBxhn5eX2KMpD3JP6TuNFdjZr7kzCMC"
                };
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://api.nacecomm.online/webauthn/authenticate/delete-passkey", content);
                if (response.StatusCode == HttpStatusCode.Created)
                {

                    return new SuccessResult<bool>();
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonObj = JObject.Parse(responseContent);
                    string message = jsonObj != null ? jsonObj["message"].ConvertToString() : "";
                    return new ErrorResult<bool>(message);

                }

            }
            return new ErrorResult<bool>("Xóa thất bại");
        }

        public async Task<FunctionResult<string>> LoginRSAsync(LoginRSRequest request)
        {
            //lấy mã code trước
            //gửi trả giao diện
            //lúc nào thành công thì gửi token sau
            //tránh lỗi timeout
            var user = await _serviceWrapper.User.User.SelectByMaButKyAsync(request.rs_ma_but_ky);
            if (user == null) return new ErrorResult<string>("Thông tin đăng nhập không hợp lệ");
            var result = await _serviceWrapper.RemoteSigningSerivce.DangNhapCodeAsync(new Model.RemoteSigning.DangNhapRequest(
                Serial: user.serial_number,
                Masothue: user.donvi_ma_dv,
                Mahethong: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Email: user.email.ConvertToString()
            ));
            if (result.is_success)
            {
                await _serviceWrapper.HoaDon.RsYeuCauKy.SaveYeuCauKyAsync(result.data, request.session_id.ConvertToString(), Model.Enum.e_rs_yeu_cau_ky_type.DANG_NHAP, user.id.ToString());
                //lưu mã code với session_id vào cache
                //lấy kết quả theo code từ backgroud job
                //push messsage khi có kết quả
                // await _serviceWrapper.Cache.SetDataAsync<string>(result.data, request.session_id.ConvertToString(), DateTime.Now.AddHours(1));
                // await _serviceWrapper.Cache.SetDataAsync<user>($"{result.data}_user", user, DateTime.Now.AddHours(1));

                // _serviceWrapper.Core.TaskQueue.EnqueueTask(async _ =>
                //      {
                //          await TryGetQuaDangNhapRsAsync(result.data);
                //      });
            }
            return result;
        }
        public async Task<bool> TryGetQuaDangNhapRsAsync(string code)
        {
            var ketQuaKy = await _serviceWrapper.RemoteSigningSerivce.TryGetKetQuaKyThenClearAsync(code);
            if (ketQuaKy.is_success)
            {
                var session_id = await _serviceWrapper.Cache.GetDataAsync<string>(code);
                var user = await _serviceWrapper.Cache.GetDataAsync<user>(code + "_user");
                LogWriter.Writer("Success", "TryGetQuaDangNhapRsAsync", "");
                LogWriter.Writer(session_id, "TryGetQuaDangNhapRsAsync", "session_id");
                try
                {
                    await _hoaDonPhatHanhHub.OnRemoteSigningSuccess(new Model.Request.Hub.RemoteSigningSuccess()
                    {
                        request_code = code,
                        user_id = session_id,
                        data = null
                    });
                }
                catch (Exception ex)
                {
                    LogWriter.Writer(ex.Message, "TryGetQuaDangNhapRsAsync", "");
                }
                if (session_id.ConvertToString() != "" && user != null)
                {
                    var profile = await this.GetProfileAsync(user.id);
                    if (profile == null) return false;
                    var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
                    {
                        full_name = profile.full_name,
                        id = profile.user_id,
                        username = profile.username,
                        donvi_ma_dv = profile.donvi_ma_dv,
                        vender_id = profile.vender_id,
                        is_hsm_signing = profile.is_hsm_signing,
                        is_remote_signing = profile.is_remote_signing,
                    });
                    var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
                    {
                        access_token = accessToken,
                        user_id = profile.user_id
                    });
                    var data = new LoginRespone()
                    {
                        is_verify_cert = true,
                        profile = profile,
                        token_info = new TokenInfo()
                        {
                            access_token = accessToken,
                            refresh_token = refreshToken
                        }
                    };
                    await _hoaDonPhatHanhHub.OnRemoteSigningSuccess(new Model.Request.Hub.RemoteSigningSuccess()
                    {
                        request_code = code,
                        user_id = session_id,
                        data = data
                    });
                }
            }
            return false;
        }

        public async Task<FunctionResult<bool>> XuLyThongDiepKyRSAsync(rs_yeu_cau_ky model)
        {
            var code = model.code;
            var session_id = model.user_id;
            var user_id = model.type_key.ConvertToInt();
            var profile = await this.GetProfileAsync(user_id);
            if (profile == null) return new ErrorResult<bool>("Dữ liệu không hợp lệ");
            var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
            {
                full_name = profile.full_name,
                id = profile.user_id,
                username = profile.username,
                donvi_ma_dv = profile.donvi_ma_dv,
                vender_id = profile.vender_id,
                is_hsm_signing = profile.is_hsm_signing,
                is_remote_signing = profile.is_remote_signing,
            });
            var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
            {
                access_token = accessToken,
                user_id = profile.user_id
            });
            var data = new LoginRespone()
            {
                is_verify_cert = true,
                profile = profile,
                token_info = new TokenInfo()
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                }
            };
            await _hoaDonPhatHanhHub.OnRemoteSigningSuccess(new Model.Request.Hub.RemoteSigningSuccess()
            {
                request_code = code,
                user_id = session_id,
                data = data
            });
            return new SuccessResult<bool>("Dữ liệu không hợp lệ");
        }

        public async Task<FunctionResult<LoginRespone>> LoginRSGetResultAsync(string uuid)
        {
            var yeuCauKyCached = await _serviceWrapper.HoaDon.RsYeuCauKy.SelectByCodeAsync(uuid);
            if (yeuCauKyCached == null) return new ErrorResult<LoginRespone>("Dữ liệu không hợp lệ");
            if (yeuCauKyCached.ket_qua_ky.ConvertToString() == "") return new ErrorResult<LoginRespone>("Chưa có kết quả ký");
            var user_id = yeuCauKyCached.type_key.ConvertToInt();
            var profile = await this.GetProfileAsync(user_id);
            if (profile == null) return new ErrorResult<LoginRespone>("Dữ liệu không hợp lệ");
            var accessToken = await _serviceWrapper.Core.JwtToken.CreateAccessTokenAsync(new JwtTokenInfo()
            {
                full_name = profile.full_name,
                id = profile.user_id,
                username = profile.username,
                donvi_ma_dv = profile.donvi_ma_dv,
                vender_id = profile.vender_id,
                is_hsm_signing = profile.is_hsm_signing,
                is_remote_signing = profile.is_remote_signing,
            });
            var refreshToken = await _serviceWrapper.Core.JwtToken.CreateRefreshTokenAsync(new JwtRefreshTokenInfo()
            {
                access_token = accessToken,
                user_id = profile.user_id
            });
            var data = new LoginRespone()
            {
                is_verify_cert = true,
                profile = profile,
                token_info = new TokenInfo()
                {
                    access_token = accessToken,
                    refresh_token = refreshToken
                }
            };
            return new SuccessResult<LoginRespone>(data);
        }
    }
}

