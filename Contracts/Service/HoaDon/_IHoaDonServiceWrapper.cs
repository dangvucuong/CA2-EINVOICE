using Contracts.Service.Base;
using Contracts.Service.HoaDon.PushMessageToVender;
using Contracts.Service.HoaDon.XuLyThongDiep;

namespace Contracts.Service.HoaDon
{
    public interface IHoaDonServiceWrapper : IBaseService
    {
        ILoaiHoaDonService LoaiHoaDon { get; }
        ILoaiHoaDonCTService LoaiHoaDonCT { get; }
        ILoaiHoaDonCTTemplateService LoaiHoaDonCTTemplate { get; }
        IMauHoaDonService MauHoaDon { get; }
        IHoaDonDangKyPhatHanhService HoaDonDangKyPhatHanh { get; }
        IHoaDonService HoaDon { get; }
        IHoaDonSendEmailService HoaDonSendEmail { get; }
        IHoaDonImportService HoaDonImport { get; }
        IHoaDonReportService HoaDonReport { get; }
        IHoaDonHangHoaService HoaDonHangHoa { get; }
        IHoaDonLogService HoaDonLog { get; }
        IHoaDonLoaiPhiService HoaDonLoaiPhi { get; }
        IXyLyThongDiepProvider XyLyThongDiepProvider { get; }
        IPushMessageToVenderService PushMessageToVender {get;}
        IHoaDonKyLoService KyLo {get;}
        IRsYeuCauKyService RsYeuCauKy {get;}
    }
}