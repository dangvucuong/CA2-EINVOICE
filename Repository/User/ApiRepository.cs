using Contract.Repository.User;
using Contracts.Repository.Base;
using Dapper;
using Model.Respone.Api;
using Model.Table;
using Repository.Base;

namespace Repository.User
{
    public class ApiRepository : CRUDRepository<api>, IApiRepository
    {
        public ApiRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<ApiItemViewModel>> SelectAllViewModelAsync(int sub_system_id)
        {
            var param = new DynamicParameters();
            param.Add("@sub_system_id", sub_system_id);
            return _dbConnection.SelectAsync<ApiItemViewModel>("api_select_viewmodel", param);

        }

        public Task<IEnumerable<api>> SelectBySubSystemAsync(int sub_system_id)
        {
            var param = new DynamicParameters();
            param.Add("@sub_system_id", sub_system_id);
            return _dbConnection.SelectAsync<api>("api_select", param);
        }

        public Task<IEnumerable<api>> SelectByUserAsync(int user_id, int sub_system_id = 0)
        {
            var param = new DynamicParameters();
            param.Add("@user_id", user_id);
            param.Add("@sub_system_id", sub_system_id);
            return _dbConnection.SelectAsync<api>("api_select_by_user", param);
        }
    }
}

