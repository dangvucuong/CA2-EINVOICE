using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Dapper;
using Model;
using Repository.Base;

namespace Repository.Category
{
    public class DonViCtsRepository : CRUDRepository<don_vi_cts>, IDonViCtsRepository
    {
        public DonViCtsRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<don_vi_cts>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<don_vi_cts>("don_vi_cts_selct_by_donvi", param);
        }
    }
}