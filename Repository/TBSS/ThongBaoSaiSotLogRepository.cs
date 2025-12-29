using Contracts.Repository.Base;
using Contracts.Repository.TBSS;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.TBSS
{
    public class ThongBaoSaiSotLogRepository : CRUDRepository<thong_bao_sai_sot_log>, IThongBaoSaiSotLogRepository
    {
        public ThongBaoSaiSotLogRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<IEnumerable<thong_bao_sai_sot_log>> SelectByThongBaoAsync(int thong_bao_sai_sot_id)
        {
            var param = new DynamicParameters();
            param.Add("@thong_bao_sai_sot_id", thong_bao_sai_sot_id);
            return _dbConnection.SelectAsync<thong_bao_sai_sot_log>("thong_bao_sai_sot_log_select_by_thongbao_id", param);
        }
    }
}