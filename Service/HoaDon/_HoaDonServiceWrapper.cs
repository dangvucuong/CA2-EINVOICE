using Contracts.Service.HoaDon;
using Contracts.Service.HoaDon.PushMessageToVender;
using Contracts.Service.HoaDon.XuLyThongDiep;
using Service.Base;
using Service.HoaDon.PushMessageToVender;
using Service.HoaDon.XuLyThongDiep;

namespace Service.HoaDon
{
    public class HoaDonServiceWrapper : BaseService, IHoaDonServiceWrapper
    {
        private ILoaiHoaDonService _loaHoaDonService;
        private ILoaiHoaDonCTService _loaHoaDonCT;
        private ILoaiHoaDonCTTemplateService _loaHoaDonCTTemplateService;
        private IMauHoaDonService _mauHoaDonService;
        private IHoaDonDangKyPhatHanhService _hoaDonDangKyPhatHanhService;
        private IHoaDonService _hoaDonService;
        private IHoaDonHangHoaService _hoaDonHangHoaService;
        private IHoaDonLogService _hoaDonLogService;
        private IHoaDonLoaiPhiService _hoaDonLoaiPhiService;
        private IXyLyThongDiepProvider _xyLyThongDiepProvider;
        private IHoaDonReportService _hoaDonReportService;
        private IHoaDonImportService _hoaDonImportService;
        private IHoaDonSendEmailService _hoaDonSendEmailService;
        private IPushMessageToVenderService _pushMessageToVenderService;
        private IHoaDonKyLoService _hoaDonKyLoService;
        private IRsYeuCauKyService _rsYeuCauKyService;
        public HoaDonServiceWrapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ILoaiHoaDonService LoaiHoaDon => _loaHoaDonService ??= new LoaiHoaDonService(_serviceProvider);

        public ILoaiHoaDonCTService LoaiHoaDonCT => _loaHoaDonCT ??= new LoaiHoaDonCTService(_serviceProvider);

        public ILoaiHoaDonCTTemplateService LoaiHoaDonCTTemplate => _loaHoaDonCTTemplateService ??= new LoaiHoaDonCTTemplateService(_serviceProvider);

        public IMauHoaDonService MauHoaDon => _mauHoaDonService ??= new MauHoaDonService(_serviceProvider);

        public IHoaDonDangKyPhatHanhService HoaDonDangKyPhatHanh => _hoaDonDangKyPhatHanhService ??= new HoaDonDangKyPhatHanhService(_serviceProvider);

        public IHoaDonService HoaDon => _hoaDonService ??= new HoaDonService(_serviceProvider);

        public IHoaDonHangHoaService HoaDonHangHoa => _hoaDonHangHoaService ??= new HoaDonHangHoaService(_serviceProvider);

        public IHoaDonLogService HoaDonLog => _hoaDonLogService ??= new HoaDonLogService(_serviceProvider);

        public IHoaDonLoaiPhiService HoaDonLoaiPhi => _hoaDonLoaiPhiService ??= new HoaDonLoaiPhiService(_serviceProvider);

        public IXyLyThongDiepProvider XyLyThongDiepProvider => _xyLyThongDiepProvider ??= new XuLyThongDiepProvider(_serviceProvider);

        public IHoaDonReportService HoaDonReport => _hoaDonReportService ?? new HoaDonReportService(_serviceProvider);

        public IHoaDonImportService HoaDonImport => _hoaDonImportService??= new HoaDonImportService(_serviceProvider);

        public IHoaDonSendEmailService HoaDonSendEmail => _hoaDonSendEmailService??= new HoaDonSendEmailService(_serviceProvider);

        public IPushMessageToVenderService PushMessageToVender => _pushMessageToVenderService??=new PushMessageToVenderService(_serviceProvider);

        public IHoaDonKyLoService KyLo => _hoaDonKyLoService??=new HoaDonKyLoService(_serviceProvider);

        public IRsYeuCauKyService RsYeuCauKy => _rsYeuCauKyService??=new RsYeuCauKyService(_serviceProvider);
    }
}