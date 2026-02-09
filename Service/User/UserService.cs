using Common;
using Contract.Repository.User;
using Contract.Service.User;
using Model;
using Model.Base;
using Model.FuncResult;
using Model.Request.Base;
using Model.Respone.Account;
using Model.Respone.User;
using Model.Table;
using Service.Base;

namespace Service.User
{
    public class UserService : CRUDService<user>, IUserService
    {
        private IUserRepository _userRepository;
        private IUserRoleService _userRoleService;
        public UserService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._userRepository = _repositoryWrapper.User.User;
            this._repositoryBase = _repositoryWrapper.User.User;
            this._userRoleService = _serviceWrapper.User.UserRole;
        }

        public async Task<FunctionResult<UserEditModel>> SaveChangeAsync(UserEditModel model)
        {
            var user_id_current = this.GetCurrentUserId();
            if (model.id > 0)
            {
                var obj = await this.SelectEditModelByIdAsync(model.id);
                obj.full_name = model.full_name;
                obj.email = model.email;
                obj.serial_number = model.serial_number;
                obj.username = model.username;
                obj.SetUpdateInfo(user_id_current);
                var isUpdated = await this.UpdateAsync(obj.Map<user>());
                if (!isUpdated) return new ErrorResult<UserEditModel>();

                var role_delete_ids = obj.role_ids.Where(x => !model.role_ids.Contains(x)).ToList();
                var role_insert_ids = model.role_ids.Where(x => !obj.role_ids.Contains(x)).ToList();

                var userRoles = await _serviceWrapper.User.UserRole.SelectByUserIdAsync(model.id);


                foreach (var role_id in role_delete_ids)
                {
                    var item = userRoles.Where(x => x.role_id == role_id).FirstOrDefault();
                    if (item != null) await _userRoleService.DeleteAsync(item.id);
                }

                foreach (var role_id in role_insert_ids)
                {
                    var item = new user_role()
                    {
                        role_id = role_id,
                        user_id = model.id
                    };
                    item.SetInsertInfo(user_id_current);
                    item.id = await _userRoleService.InsertAsync(item);
                }

                return new SuccessResult<UserEditModel>(obj);
            }
            else
            {
                var user = model.Map<user>();
                user.SetInsertInfo(user_id_current);
                user.id = await this.InsertAsync(user);
                if (user.id > 0)
                {
                    model.id = user.id;
                    foreach (var role_id in model.role_ids)
                    {
                        var item = new user_role()
                        {
                            role_id = role_id,
                            user_id = user.id
                        };
                        item.SetInsertInfo(user_id_current);
                        item.id = await _userRoleService.InsertAsync(item);
                    }
                    return new SuccessResult<UserEditModel>(model);
                }
                else
                {
                    return new ErrorResult<UserEditModel>("Error!");
                }
            }
        }

        public async Task<JwtTokenInfo> SelectAndFormatJwtTokenAsync(int id)
        {
            var obj = await this.SelectByIdAsync(id);
            if (obj != null) return obj.Map<JwtTokenInfo>();
            return null;
        }

        public async Task<PagingResult<IEnumerable<user>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            return await _userRepository.SelectByDonViAsync(donvi_ma_dv, pagingRequest);
        }

        public Task<user> SelectByEmailAsync(string donvi_ma_dv, string email)
        {
            return _userRepository.SelectByEmailAsync(donvi_ma_dv, email);
        }

        public Task<user> SelectByMaButKyAsync(string rs_ma_but_ky)
        {
            return _userRepository.SelectByMaButKyAsync(rs_ma_but_ky);
        }

        public Task<user> SelectBySerialAsync(string serial, string mst)
        {
            return _userRepository.SelectBySerialAsync(serial, mst);
        }

        public Task<user> SelectByUsernameAsync(string donvi_ma_dv, string username)
        {
            return _userRepository.SelectByUsernameAsync(donvi_ma_dv, username);

        }

        public async Task<UserEditModel> SelectEditModelByIdAsync(int id)
        {
            var user = await this.SelectByIdAsync(id);
            var userRoles = await _serviceWrapper.User.UserRole.SelectByUserIdAsync(id);
            var model = user.Map<UserEditModel>();
            model.role_ids = userRoles.Select(x => x.role_id).ToList();
            return model;
        }

        public async Task<bool> SyncFromCtsAsync(don_vi_cts obj)
        {
            if (obj.is_active)
            {
                var users = await _userRepository.SelectByDonViAsync(obj.donvi_ma_dv, new PagingRequest()
                {
                    page_index = 1,
                    page_size = 99999
                });
                var user = users.data.Where(x => x.serial_number == obj.serial_number).FirstOrDefault();
                if (user != null) return true;
                var donVi = await _serviceWrapper.Category.DonVi.SelectByMaDonViAsync(obj.donvi_ma_dv);
                if (user != null) return false;
                var userId = this.GetCurrentUserId();
                user = new user()
                {
                    donvi_ma_dv = obj.donvi_ma_dv,
                    email = donVi.email,
                    full_name = donVi.ten_dv,
                    is_hsm_signing = false,
                    is_serial_remote_signing_verified = false,
                    rs_ma_but_ky = null,
                    serial_number = obj.serial_number,
                    username = obj.serial_number,
                    is_active = true

                };
                user.SetInsertInfo(userId);
                user.id = await this.InsertAsync(user);
                var user_role = new user_role()
                {
                    role_id = 10,
                    user_id = user.id
                };
                user_role.SetInsertInfo(userId);
                user_role.id = await _userRoleService.InsertAsync(user_role);
                return true;

            }
            return true;
        }

        public async Task<FunctionResult<bool>> UpdateRemoteSigningSerialAsync(UserUpdateRemoteSigningSerialNumberRequest model)
        {
            var user = await this.SelectByIdAsync(model.GetUserId());
            if (user == null) return new ErrorResult<bool>("Dữ liệu không hợp lệ");
            //
            var rsServerInfo = await _serviceWrapper.RemoteSigningSerivce.GetCertInfoAsync(model.rs_ma_but_ky.ConvertToInt());
            if (rsServerInfo.is_success)
            {
                user.rs_ma_but_ky = model.rs_ma_but_ky;
                user.is_serial_remote_signing_verified = true;
                user.SetUpdateInfo(user.id);
                var isUpdated = await this.UpdateAsync(user);
                if (!isUpdated) return new ErrorResult<bool>("Cập nhật thất bại");
            }
            else
            {
                user.rs_ma_but_ky = "";
                user.is_serial_remote_signing_verified = false;
                user.SetUpdateInfo(user.id);
                var isUpdated = await this.UpdateAsync(user);
                if (!isUpdated) return new ErrorResult<bool>("Cập nhật thất bại");
            }
            //

            return new SuccessResult<bool>();
        }

        public async Task<FunctionResult<bool>> UpdateSerialNumberAsync(UserUpdateSerialNumberRequest model)
        {
            var user = await this.SelectByIdAsync(model.GetUserId());
            if (user == null) return new ErrorResult<bool>("Dữ liệu không hợp lệ");
            if (user.serial_number.ConvertToString() != "") return new ErrorResult<bool>("Chỉ gán cho user chưa có số serial");
            user.serial_number = model.serial;
            user.SetUpdateInfo(user.id);
            var isUpdated = await this.UpdateAsync(user);
            if (!isUpdated) return new ErrorResult<bool>("Cập nhật thất bại");
            return new SuccessResult<bool>();
        }

        public async Task<FunctionResult<bool>> ValidateUserExited(UserEditModel model)
        {
            //nếu tồn tại user khác cùng username + mã đơn vị thì không hợp lệ
            var user = await _userRepository.SelectByUsernameAsync(model.donvi_ma_dv, model.username);
            if (user != null)
            {
                //if (user.id != model.id) return new ErrorResult<bool>("Username đã tồn tại trong cùng cơ sở");
            }
            return new SuccessResult<bool>();
        }


        // RemoveUserAsync
        public async Task<FunctionResult<string>> RemoveUserAsync(int id)
        {
            var currentUserId = this.GetCurrentUserId();

            UserDeleteResult res =
                await _userRepository.RemoveUserAsync(id, currentUserId);

            switch (res.StatusCode)
            {
                case 0:
                    return new SuccessResult<string>(message: res.Message);

                case 1:
                case 2:
                case 99:
                    return new ErrorResult<string>(res.Message);

                default:
                    return new ErrorResult<string>("Lỗi không xác định.");
            }
        }
    }
}

