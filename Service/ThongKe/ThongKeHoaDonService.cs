using Contracts.Service.ThongKe;
using Model.Request.ThongKe;
using Model.Respone.ThongKe;
using Service.Base;

namespace Service.ThongKe
{
    public class ThongKeHoaDonService : BaseService, IThongKeHoaDonService
    {
        public ThongKeHoaDonService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoGiaTriHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request)
        {
            return _repositoryWrapper.HoaDon.HoaDon.GetTopKhachHangBySoGiaTriHDAsync(request);
        }

        public Task<IEnumerable<ThongKeTopKhachHangTheoSoLuongHoaDonRespone>> GetTopKhachHangBySoLuongHDAsync(ThongKeTopKhachHangTheoHoaDonRequest request)
        {
            return _repositoryWrapper.HoaDon.HoaDon.GetTopKhachHangBySoLuongHDAsync(request);

        }
    }
}