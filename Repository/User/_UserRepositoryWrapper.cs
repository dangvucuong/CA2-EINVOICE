using Contract.Repository.User;
using Contracts.Repository.Base;
using Contracts.Repository.User;
using Repository.Base;

namespace Repository.User
{
    public class UserRepositoryWrapper : BaseRepositoryWrapper, IUserRepositoryWrapper
    {
        private IUserRepository _userRepository;
        private IUserRoleRepository _userRoleRepository;
        private ISubSystemRepository _subSystemRepository;
        private IRoleSubSystemRepository _roleSubSystemRepository;
        private IRoleRepository _roleRepository;
        private IApiRepository _apiRepository;
        private IMenuRepository _menuRepository;
        private IRoleApiRepository _roleApiRepository;
        private IOTPRepository _oTPRepository;
        private ILogRepository _logRepository;
        private IVenderRepository _venderRepository;
        public UserRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }


        public IUserRepository User => _userRepository ??= new UserRepository(_defaultConnection);

        public IUserRoleRepository UserRole => _userRoleRepository ??= new UserRoleRepository(_defaultConnection);


        public ISubSystemRepository SubSystem => _subSystemRepository ??= new SubSystemRepository(_defaultConnection);

        public IRoleSubSystemRepository RoleSubSystem => _roleSubSystemRepository ??= new RoleSubSystemRepository(_defaultConnection);


        public IRoleRepository Role => _roleRepository ??= new RoleRepository(_defaultConnection);

        public IApiRepository Api => _apiRepository ??= new ApiRepository(_defaultConnection);

        public IMenuRepository Menu => _menuRepository ??= new MenuRepository(_defaultConnection);

        public IRoleApiRepository RoleApi => _roleApiRepository ??= new RoleApiRepository(_defaultConnection);

        public IOTPRepository OTP => _oTPRepository ??= new OTPRepository(_defaultConnection);

        public ILogRepository Log => _logRepository ??= new LogRepository(_connectionStrings.Log);

        public IVenderRepository Vender => _venderRepository ??= new VenderRepository(_defaultConnection);
    }
}

