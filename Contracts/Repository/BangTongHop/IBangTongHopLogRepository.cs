using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.BangTongHop
{
    public interface IBangTongHopLogRepository : ICRUDRepository<bang_tong_hop_du_lieu_log>
    {
        Task<IEnumerable<bang_tong_hop_du_lieu_log>> SelectByBangTongHopIdAsync(int bangTongHopId);
    }
}