using Contracts.Service.Base;
using Model.Table;

namespace Contracts.Service.HoaDon.XuLyThongDiep
{
    public interface IXyLyThongDiepProvider : IBaseService
    {
        Task<IXuLyThongDiepService> GetServiceAsync(hoa_don hoaDon);
    }
}