using Contracts.Service.Base;
using Model.Base;
using Model.Table;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonLogService : ICRUDService<hoa_don_log>
    {
        Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id);
        Task<IEnumerable<hoa_don_log>> SelectByHoaDonAsync(int hoa_don_id, int hoa_don_log_type_id);
        Task<FunctionResult<string>> SaveFromPhatHanhAsync(int hoa_don_id, string noi_dung_thuc_hien, string xmlResult, bool isCQTChapNhan);
        Task<FunctionResult<string>> SaveFromPhatHanhBangKeAsync(int hoa_don_id, string noi_dung_thuc_hien, string fileXmlPath, bool isCQTChapNhan);
    }
}