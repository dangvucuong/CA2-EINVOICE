using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.TBSS
{
    public interface IThongBaoSaiSotLogService : ICRUDService<thong_bao_sai_sot_log>
    {
        Task<IEnumerable<thong_bao_sai_sot_log>> SelectByThongBaoIdAsync(int thong_bao_sai_sot_id);
        
    }
}