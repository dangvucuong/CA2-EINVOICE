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

        public Task<IEnumerable<int>> SelectUsedHoaDonIdsByDonViAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHopHoaDon.SelectUsedHoaDonIdsByDonViAsync(donvi_ma_dv);
        }

        public Task<IEnumerable<hoa_don>> SelectHoaDonForTongHopAsync(string donvi_ma_dv, DateTime? tu_ngay, DateTime? den_ngay)
        {
            return _repositoryWrapper.BangTongHopDuLieu.BangTongHopHoaDon.SelectHoaDonForTongHopAsync(donvi_ma_dv, tu_ngay, den_ngay);
        }
    }
}