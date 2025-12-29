using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.BangTongHop
{
    public interface IBangTongHopRepository : ICRUDRepository<bang_tong_hop_du_lieu>
    {
        Task<IEnumerable<bang_tong_hop_du_lieu>> SelectByDonViAsync(string donvi_ma_dv);
    }
}