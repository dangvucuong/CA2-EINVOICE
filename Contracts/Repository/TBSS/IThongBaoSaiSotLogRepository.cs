using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.TBSS
{
    public interface IThongBaoSaiSotLogRepository : ICRUDRepository<thong_bao_sai_sot_log>
    {
        Task<IEnumerable<thong_bao_sai_sot_log>> SelectByThongBaoAsync(int thong_bao_sai_sot_id);
    }
}