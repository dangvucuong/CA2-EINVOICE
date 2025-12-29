using Contracts.Repository.Base;
using Contracts.Repository.TBSS;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.TBSS
{
    public class ThongBaoSaiSotRepository : CRUDRepository<thong_bao_sai_sot>, IThongBaoSaiSotRepository
    {
        public ThongBaoSaiSotRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<thong_bao_sai_sot>> SelectByDonViAsync(string donvi_ma_dv)
        {
            var param = new DynamicParameters();
            param.Add("@donvi_ma_dv", donvi_ma_dv);
            return _dbConnection.SelectAsync<thong_bao_sai_sot>("thong_bao_sai_sot_select_by_donvi", param);
        }

        public Task<thong_bao_sai_sot> SelectByPhatHanhUuidAsync(string phat_hanh_uuid)
        {
            var param = new DynamicParameters();
            param.Add("@phat_hanh_uuid", phat_hanh_uuid);
            return _dbConnection.SelectFirstOrDefaultAsync<thong_bao_sai_sot>("thong_bao_sai_sot_select_by_ohathanh_uuid", param);
        }
    }
}