using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonLoaiPhiService:ICRUDService<hoa_don_loai_phi>
    {
        Task<IEnumerable<hoa_don_loai_phi>> SelectByHoaDonAsync(int hoa_don_id);
    }
}