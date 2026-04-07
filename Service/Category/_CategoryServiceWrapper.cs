using Contract.Service.Category;
using Contracts.Service.Category;
using Service.Base;

namespace Service.Category
{
    public class CategoryServiceWrapper : BaseService, ICategoryServiceWrapper
    {
        private IDonViService _donViService;
        private IKhachHangService _khachHangService;
        private IHangHoaService _hangHoaService;
        private IDonViMuaChuKySoService _donViMuaChuKySoService;
        private ICoQuanThueService _coQuanThueService;
        private IWatermarkService _watermarkService;
        private IDaiLyService _daiLyService;
        private IDonViCtsService _donViCtsService;
        public CategoryServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IDonViService DonVi => _donViService ??= new DonViService(_serviceProvider);

        public IKhachHangService KhachHang => _khachHangService ??= new KhachHangService(_serviceProvider);

        public IHangHoaService HangHoa => _hangHoaService ??= new HangHoaService(_serviceProvider);

        public IDonViMuaChuKySoService DonViMuaChuKySo => _donViMuaChuKySoService??= new DonViMuaChuKySoService(_serviceProvider);

        public ICoQuanThueService CoQuanThue => _coQuanThueService??= new CoQuanThueService(_serviceProvider);

        public IWatermarkService Watermark => _watermarkService??= new WatermarkService(_serviceProvider);

        public IDaiLyService DaiLy => _daiLyService??=new DaiLyService(_serviceProvider);

        public IDonViCtsService DonViCts => _donViCtsService ??=new DonViCtsService(_serviceProvider);
    }
}

