using Contracts.Service.Base;
using Contracts.Service.User;

namespace Contract.Service.User
{
    public interface IUserServiceWrapper : IBaseService
    {
        IUserService User { get; }
        IUserRoleService UserRole { get; }
        ISubSystemService SubSystem { get; }
        IRoleService Role { get; }
        IRoleSubSystemService RoleSubSystem { get; }
        IApiService Api { get; }
        IMenuService Menu { get; }
        IRoleApiService RoleApi { get; }
        IOTPService OTP { get; }
        ILogService Log { get; }
        IVenderService Vender { get; }

    }
}

