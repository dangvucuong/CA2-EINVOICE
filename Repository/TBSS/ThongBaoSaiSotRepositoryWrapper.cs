using Contracts.Repository.Base;
using Contracts.Repository.TBSS;
using Repository.Base;

namespace Repository.TBSS
{
    public class ThongBaoSaiSotRepositoryWrapper : BaseRepositoryWrapper, IThongBaoSaiSotRepositoryWrapper
    {
        private IThongBaoSaiSotRepository _thongBaoSaiSotRepository;
        private IThongBaoSaiSotChiTietRepository _thongBaoSaiSotChiTietRepository;
        private IThongBaoSaiSotLogRepository _thongBaoSaiSotLogRepository;
        public ThongBaoSaiSotRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public IThongBaoSaiSotRepository ThongBaoSaiSot => _thongBaoSaiSotRepository ??= new ThongBaoSaiSotRepository(_defaultConnection);

        public IThongBaoSaiSotChiTietRepository ThongBaoSaiSotChiTiet => _thongBaoSaiSotChiTietRepository ??= new ThongBaoSaiSotChiTietRepository(_defaultConnection);

        public IThongBaoSaiSotLogRepository ThongBaoSaiSotLog => _thongBaoSaiSotLogRepository ??= new ThongBaoSaiSotLogRepository(_defaultConnection);
    }
}