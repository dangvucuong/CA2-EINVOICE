using Contracts.Repository.Base;
using Contracts.Repository.ToKhai;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.ToKhai
{
    public class ToKhaiRepository : CRUDRepository<to_khai>, IToKhaiRepository
    {
        public ToKhaiRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<to_khai>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<to_khai>("to_khai_select_by_donvi", param);
        }

        public Task<to_khai> SelectByPhatHanhUuidAsync(string phat_hanh_uuid)
        {
            var param = new DynamicParameters();
            param.Add("@phat_hanh_uuid", phat_hanh_uuid);
            return _dbConnection.SelectFirstOrDefaultAsync<to_khai>("to_khai_select_by_phathanh_uuid", param);
        }
    }
}