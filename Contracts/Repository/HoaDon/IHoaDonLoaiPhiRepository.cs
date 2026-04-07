using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonLoaiPhiRepository : ICRUDRepository<hoa_don_loai_phi>
    {
        Task<IEnumerable<hoa_don_loai_phi>> SelectByHoaDonAsync(int hoa_don_id);
    }
}