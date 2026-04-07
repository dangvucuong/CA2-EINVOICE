using Common;
using Contracts.Repository.Base;
using Contracts.Repository.ToKhai;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.ToKhai
{
    public class ToKhaiLogRepository : CRUDRepository<to_khai_log>, IToKhaiLogRepository
    {
        public ToKhaiLogRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<to_khai_log>> SelectByToKhaiAsync(int to_khai_id)
        {
            var param = new DynamicParameters();
            param.Add("@to_khai_id", to_khai_id);
            return _dbConnection.SelectAsync<to_khai_log>("to_khai_log_select_by_to_khai", param);
        }

        public Task<IEnumerable<to_khai_log>> SelectByToKhaiIdsAsync(List<int> to_khai_ids)
        {
            var param = new DynamicParameters();
            param.Add("@ids", to_khai_ids.ConvertToTableValuedParameter());
            return _dbConnection.SelectAsync<to_khai_log>("to_khai_log_select_by_to_khais", param);
        }
    }
}