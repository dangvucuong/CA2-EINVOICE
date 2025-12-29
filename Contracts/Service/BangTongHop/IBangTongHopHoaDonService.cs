using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.BangTongHop
{
    public interface IBangTongHopHoaDonService : ICRUDService<bang_tong_hop_du_lieu_hoa_don>
    {
        Task<IEnumerable<bang_tong_hop_du_lieu_hoa_don>> SelectByBangTongHopAsync(int bangTongHopId);
        Task<bool> InsertsAsync(IEnumerable<bang_tong_hop_du_lieu_hoa_don> duLieuHoaDons);
        Task<bool> DeletesAsync(IEnumerable<int> ids, int user_id);
    }
}