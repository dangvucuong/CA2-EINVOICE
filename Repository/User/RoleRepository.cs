using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.Respone.Role;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class RoleRepository : CRUDRepository<role>, IRoleRepository
    {
        public RoleRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<RoleViewModel>> SelectAllViewModalAsync()
        {
            return _dbConnection.SelectAsync<RoleViewModel>("role_select_all_viewmodel");
        }

        public Task<IEnumerable<role>> SelectByUserAsync(int user_id)
        {
            var param = new DynamicParameters();
            param.Add("@user_id", user_id);
            return _dbConnection.SelectAsync<role>("role_select_by_user", param);
        }
    }
}

