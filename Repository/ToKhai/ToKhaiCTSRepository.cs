using Contracts.Repository.Base;
using Contracts.Repository.ToKhai;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.ToKhai
{
    public class ToKhaiCTSRepository : CRUDRepository<to_khai_cts>, IToKhaiCTSRepository
    {
        public ToKhaiCTSRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<to_khai_cts>> SelectByToKhaiAsync(int to_khai_id)
        {
            var param = new DynamicParameters();
            param.Add("@to_khai_id", to_khai_id);
            return _dbConnection.SelectAsync<to_khai_cts>("to_khai_cts_select_by_to_khai", param);
        }
    }
}