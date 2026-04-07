using Contracts.Repository.Base;

namespace Contracts.Repository.TBSS
{
    public interface IThongBaoSaiSotRepositoryWrapper : IBaseRepositoryWrapper
    {
        IThongBaoSaiSotRepository ThongBaoSaiSot { get; }
        IThongBaoSaiSotChiTietRepository ThongBaoSaiSotChiTiet { get; }
        IThongBaoSaiSotLogRepository ThongBaoSaiSotLog { get; }
    }
}