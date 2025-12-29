using Contracts.Service.TBSS;
using Service.Base;

namespace Service.TBSS
{
    public class ThongBaoSaiSotServiceWrapper :BaseService, IThongBaoSaiSotServiceWrapper
    {
        private IThongBaoSaiSotService _thongBaoSaiSotService;
        private IThongBaoSaiSotChiTietService _thongBaoSaiSotChiTietService;
        private IThongBaoSaiSotLogService _thongBaoSaiSotLogService;
        public ThongBaoSaiSotServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IThongBaoSaiSotService ThongBaoSaiSot => _thongBaoSaiSotService ??=new ThongBaoSaiSotService(_serviceProvider);

        public IThongBaoSaiSotChiTietService ThongBaoSaiSotChiTiet => _thongBaoSaiSotChiTietService ??=new ThongBaoSaiSotChiTietService(_serviceProvider);

        public IThongBaoSaiSotLogService ThongBaoSaiSotLog => _thongBaoSaiSotLogService??= new ThongBaoSaiSotLogService(_serviceProvider);
    }
}