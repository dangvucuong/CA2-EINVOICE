using Contracts.Service.HoaDon;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class HoaDonLoaiPhiService : CRUDService<hoa_don_loai_phi>, IHoaDonLoaiPhiService
    {
        public HoaDonLoaiPhiService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.HoaDonLoaiPhi;
        }

        public Task<IEnumerable<hoa_don_loai_phi>> SelectByHoaDonAsync(int hoa_don_id)
        {
            return _repositoryWrapper.HoaDon.HoaDonLoaiPhi.SelectByHoaDonAsync(hoa_don_id);
        }
    }
}