using Contracts.Repository.Base;
using Model.Table;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonLogRepository : ICRUDRepository<hoa_don_log>
    {
        Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id);
        Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id, int hoa_don_log_type_id);
    }
}