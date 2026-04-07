using Contracts.Repository.Base;
using Contracts.Repository.HoaDon;
using Repository.Base;

namespace Repository.HoaDon
{
    public class HoaDonRepositoryWrapper : BaseRepositoryWrapper, IHoaDonRepositoryWrapper
    {
        private ILoaiHoaDonRepository _loaiHoaDonRepository;
        private ILoaiHoaDonCTRepository _loaiHoaDonCT;
        private ILoaiHoaDonCTTemplateRepository _loaiHoaDonCTTemplateRepository;
        private IMauHoaDonRepository _mauHoDon;
        private IHoaDonDangKyPhatHanhRepository _hoaDonDangKyPhatHanhRepository;
        private IHoaDonRepository _hoaDonRepository;
        private IHoaDonHangHoaRepostiory _hoaDonHangHoaRepostiory;
        private IHoaDonLogRepository _hoaDonLogRepository;
        private IHoaDonLoaiPhiRepository _hoaDonLoaiPhiRepository;
        private IHoaDonReportRepository _hoaDonReportRepository;
        private IPhatHanhUUIDRepository _phatHanhUUIDRepository;
        private IRsYeuCauKyRepository _rsYeuCauKyRepository;
        public HoaDonRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public ILoaiHoaDonRepository LoaiHoaDon => _loaiHoaDonRepository ??= new LoaiHoaDonRepository(_defaultConnection);

        public ILoaiHoaDonCTRepository LoaiHoaDonCT => _loaiHoaDonCT ?? new LoaiHoaDonCTRepository(_defaultConnection);

        public ILoaiHoaDonCTTemplateRepository LoaiHoaDonCTTemplate => _loaiHoaDonCTTemplateRepository ??= new LoaiHoaDonCTTemplateRepository(_defaultConnection);

        public IMauHoaDonRepository MauHoaDon => _mauHoDon ??= new MauHoaDonRepository(_defaultConnection);

        public IHoaDonDangKyPhatHanhRepository HoaDonDangKyPhatHanh => _hoaDonDangKyPhatHanhRepository ??= new HoaDonDangKyPhatHanhRepository(_defaultConnection);

        public IHoaDonRepository HoaDon => _hoaDonRepository ??= new HoaDonRepository(_defaultConnection);

        public IHoaDonHangHoaRepostiory HoaDonHangHoa => _hoaDonHangHoaRepostiory ??= new HoaDonHangHoaRepostiory(_defaultConnection);

        public IHoaDonLogRepository HoaDonLog => _hoaDonLogRepository ??= new HoaDonLogRepository(_connectionStrings.Log);

        public IHoaDonLoaiPhiRepository HoaDonLoaiPhi => _hoaDonLoaiPhiRepository ??= new HoaDonLoaiPhiRepository(_defaultConnection);

        public IHoaDonReportRepository HoaDonReport => _hoaDonReportRepository ??= new HoaDonReportRepository(_defaultConnection);

        public IPhatHanhUUIDRepository PhatHanhUUID => _phatHanhUUIDRepository??=new PhatHanhUUIDRepository(_connectionStrings.Log);

        public IRsYeuCauKyRepository RsYeuCauKyRepository => _rsYeuCauKyRepository??=new RsYeuCauKyRepository(_connectionStrings.Log);
    }
}