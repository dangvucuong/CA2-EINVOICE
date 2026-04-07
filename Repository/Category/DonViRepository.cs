using Contract.Repository.Category;
using Contracts.Repository.Base;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.Category
{
    public class DonViRepository : CRUDRepository<donvi>, IDonViRepository
    {
        public DonViRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<int> CalculateTongCKSConLaiAsync(string ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@ma_dv", ma_dv);
            return _dbConnection.SelectFirstOrDefaultAsync<int>("donvi_select_calculate_cks_con_lai", param);
        }

        public Task<donvi> SelectByMaDonViAsync(string ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@ma_dv", ma_dv);
            return _dbConnection.SelectFirstOrDefaultAsync<donvi>("donvi_select_by_ma_dv", param);
        }
    }
}