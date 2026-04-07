using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.Respone.SubSystem;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class SubSystemRepository : CRUDRepository<sub_system>, ISubSystemRepository
    {
        public SubSystemRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<SubSystemItemViewModel>> SelectAllViewModelAsync()
        {
            return _dbConnection.SelectAsync<SubSystemItemViewModel>("sub_system_select_all_viewmodel");
        }

        public Task<IEnumerable<sub_system>> SelectByUserAsync(int user_id)
        {
            var param = new DynamicParameters();
            param.Add("@user_id", user_id);
            return _dbConnection.SelectAsync<sub_system>("sub_system_select_by_user", param);
        }
    }
}

