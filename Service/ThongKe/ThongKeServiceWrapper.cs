using Contracts.Service.ThongKe;
using Service.Base;

namespace Service.ThongKe
{
    public class ThongKeServiceWrapper : BaseService, IThongKeServiceWrapper
    {
        private IThongKeHoaDonService _thongKeHoaDonService;
        public ThongKeServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IThongKeHoaDonService ThongKeHoaDon => _thongKeHoaDonService ??= new ThongKeHoaDonService(_serviceProvider);
    }
}