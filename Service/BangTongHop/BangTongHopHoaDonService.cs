using Contracts.Service.BangTongHop;
using Model.Table;
using Service.Base;

namespace Service.BangTongHop
{
    public class BangTongHopHoaDonService : CRUDService<bang_tong_hop_du_lieu_hoa_don>, IBangTongHopHoaDonService
    {
        public BangTongHopHoaDonService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.BangTongHopDuLieu.BangTongHopHoaDon;
        }

        public Task<bool> DeletesAsync(IEnumerable<int> ids, int user_id)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHopHoaDon.DeletesAsync(ids, user_id);
        }

        public Task<bool> InsertsAsync(IEnumerable<bang_tong_hop_du_lieu_hoa_don> duLieuHoaDons)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHopHoaDon.InsertsAsync(duLieuHoaDons);
        }

        public Task<IEnumerable<bang_tong_hop_du_lieu_hoa_don>> SelectByBangTongHopAsync(int bangTongHopId)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHopHoaDon.SelectByBangTongHopAsync(bangTongHopId);
        }
    }
}