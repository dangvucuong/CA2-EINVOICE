using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class RoleSubSystemRepository : CRUDRepository<role_sub_system>, IRoleSubSystemRepository
    {
        public RoleSubSystemRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<role_sub_system>> SelectByRoleAsync(int role_id)
        {
            var param = new DynamicParameters();
            param.Add("@role_id", role_id);
            return _dbConnection.SelectAsync<role_sub_system>("role_sub_system_select_by_role",param);
        }
    }
}

