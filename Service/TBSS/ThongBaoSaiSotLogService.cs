using Contracts.Service.TBSS;
using Model.Table;
using Service.Base;

namespace Service.TBSS
{
    public class ThongBaoSaiSotLogService : CRUDService<thong_bao_sai_sot_log>, IThongBaoSaiSotLogService
    {
        public ThongBaoSaiSotLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog;
        }

        public Task<IEnumerable<thong_bao_sai_sot_log>> SelectByThongBaoIdAsync(int thong_bao_sai_sot_id)
        {
            return _repositoryWrapper.ThongBaoSaiSot.ThongBaoSaiSotLog.SelectByThongBaoAsync(thong_bao_sai_sot_id);
        }
    }
}