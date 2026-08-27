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

        public Task<donvi_mua_chukyso> SelectByMstAndTransactionIdAsync(string donvi_mst, string transaction_id)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_mst", donvi_mst);
            param.Add("@transaction_id", transaction_id);
            var sql = @"SELECT * FROM donvi_mua_chukyso
                WHERE donvi_mst = @donvi_mst AND transaction_id = @transaction_id";
            return _dbConnection.QueryFirstOrDefaultAsync<donvi_mua_chukyso>(sql, param);
        }
    }
}