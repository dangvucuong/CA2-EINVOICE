using Contracts.Repository.Base;

namespace Contracts.Repository.HoaDon
{
    public interface IHoaDonRepositoryWrapper : IBaseRepositoryWrapper
    {
        ILoaiHoaDonRepository LoaiHoaDon { get; }
        ILoaiHoaDonCTRepository LoaiHoaDonCT { get; }
        ILoaiHoaDonCTTemplateRepository LoaiHoaDonCTTemplate { get; }
        IMauHoaDonRepository MauHoaDon { get; }
        IHoaDonDangKyPhatHanhRepository HoaDonDangKyPhatHanh { get; }
        IHoaDonRepository HoaDon { get; }
        IHoaDonReportRepository HoaDonReport { get; }
        IHoaDonHangHoaRepostiory HoaDonHangHoa { get; }
        IHoaDonLogRepository HoaDonLog { get; }
        IHoaDonLoaiPhiRepository HoaDonLoaiPhi { get; }
        IPhatHanhUUIDRepository PhatHanhUUID { get; }
        IRsYeuCauKyRepository RsYeuCauKyRepository { get; }
    }
}