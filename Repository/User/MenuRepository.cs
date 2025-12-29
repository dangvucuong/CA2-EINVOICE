using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class MenuRepository : CRUDRepository<menu>, IMenuRepository
    {
        public MenuRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<menu>> SelectByUserAsync(int user_id, int sub_system_id = 0)
        {
            var param = new DynamicParameters();
            param.Add("@user_id", user_id);
            param.Add("@sub_system_id", sub_system_id);
            return _dbConnection.SelectAsync<menu>("menu_select_by_user", param);
        }
    }
}

