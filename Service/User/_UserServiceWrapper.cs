using Contract.Service.User;
using Contracts.Service.User;
using Service.Base;

namespace Service.User
{
    public class UserServiceWrapper : BaseService, IUserServiceWrapper
    {
        private IUserService _userService;
        private IUserRoleService _userRoleService;

        private ISubSystemService _subSystemService;
        private IRoleService _roleService;
        private IApiService _apiService;
        private IMenuService _menuService;
        private IRoleSubSystemService _roleSubSystemService;
        private IRoleApiService _roleApiService;
        private IOTPService _oTPService;
        private ILogService _logService;
        private IVenderService _venderService;
        public UserServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }


        public IUserService User => _userService ??= new UserService(_serviceProvider);

        public IUserRoleService UserRole => _userRoleService ??= new UserRoleService(_serviceProvider);



        public ISubSystemService SubSystem => _subSystemService ??= new SubSystemService(_serviceProvider);

        public IRoleService Role => _roleService ??= new RoleService(_serviceProvider);

        public IApiService Api => _apiService ??= new ApiService(_serviceProvider);

        public IMenuService Menu => _menuService ??= new MenuService(_serviceProvider);

        public IRoleSubSystemService RoleSubSystem => _roleSubSystemService ??= new RoleSubSystemService(_serviceProvider);

        public IRoleApiService RoleApi => _roleApiService ??= new RoleApiService(_serviceProvider);

        public IOTPService OTP => _oTPService ??= new OTPService(_serviceProvider);

        public ILogService Log => _logService ??= new LogService(_serviceProvider);

        public IVenderService Vender => _venderService??=new VenderService(_serviceProvider);
    }
}

