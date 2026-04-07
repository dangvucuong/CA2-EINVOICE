using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class UserRoleRepository : CRUDRepository<user_role>, IUserRoleRepository
    {
        public UserRoleRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<user_role>> SelectByUserIdAsync(int user_id)
        {
            var param = new DynamicParameters();
            param.Add("@user_id", user_id);
            return _dbConnection.SelectAsync<user_role>("user_role_select_by_user", param);
        }
    }
}

