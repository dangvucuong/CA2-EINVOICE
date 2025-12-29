using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.BangTongHop
{
    public interface IBangTongHopLogService : ICRUDService<bang_tong_hop_du_lieu_log>
    {
        Task<IEnumerable<bang_tong_hop_du_lieu_log>> SelectByBangTongHopIdAsync(int bangTongHopId);
    }
}