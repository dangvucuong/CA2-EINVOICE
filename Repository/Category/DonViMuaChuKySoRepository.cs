using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.Category
{
    public class DonViMuaChuKySoRepository : CRUDRepository<donvi_mua_chukyso>, IDonViMuaChuKySoRepository
    {
        public DonViMuaChuKySoRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<donvi_mua_chukyso>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<donvi_mua_chukyso>("donvi_mua_chukyso_select", param);
        }
    }
}