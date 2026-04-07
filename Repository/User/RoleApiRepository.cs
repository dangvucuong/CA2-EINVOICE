using Contracts.Repository.Base;
using Contracts.Repository.User;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class RoleApiRepository : CRUDRepository<role_api>, IRoleApiRepository
    {
        public RoleApiRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<role_api>> SelectByRoleAsync(int role_id, int sub_system_id)
        {
            var param = new DynamicParameters();
            param.Add("role_id",role_id);
            param.Add("sub_system_id",sub_system_id);
            return _dbConnection.SelectAsync<role_api>("role_api_select_by_role",param);
        }
    }
}