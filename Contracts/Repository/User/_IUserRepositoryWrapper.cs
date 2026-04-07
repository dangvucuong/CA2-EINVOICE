using Contracts.Repository.Base;
using Contracts.Repository.User;

namespace Contract.Repository.User
{
    public interface IUserRepositoryWrapper : IBaseRepositoryWrapper
    {
        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        ISubSystemRepository SubSystem { get; }
        IRoleSubSystemRepository RoleSubSystem { get; }
        IRoleRepository Role { get; }
        IApiRepository Api { get; }
        IMenuRepository Menu { get; }
        IRoleApiRepository RoleApi { get; }
        IOTPRepository OTP { get; }
        ILogRepository Log { get; }
        IVenderRepository Vender {get;}
    }
}

