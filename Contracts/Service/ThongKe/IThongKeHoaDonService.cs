using Contracts.Service.Base;
using Model.Request.ThongKe;
using Model.Respone.ThongKe;

namespace Contracts.Service.ThongKe
{
    public interface IThongKeHoaDonService:IBaseService
    {
        Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoLuongHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request);
        Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoGiaTriHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request);
    }
}
