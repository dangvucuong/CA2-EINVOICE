using Contracts.Service.Base;

namespace Contracts.Service.TBSS
{
    public interface IThongBaoSaiSotServiceWrapper : IBaseService
    {
        IThongBaoSaiSotService ThongBaoSaiSot { get; }
        IThongBaoSaiSotChiTietService ThongBaoSaiSotChiTiet { get; }
        IThongBaoSaiSotLogService ThongBaoSaiSotLog { get; }
    }
}