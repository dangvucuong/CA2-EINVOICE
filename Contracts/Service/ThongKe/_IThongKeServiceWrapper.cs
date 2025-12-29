using Contracts.Service.Base;

namespace Contracts.Service.ThongKe
{
    public interface IThongKeServiceWrapper:IBaseService
    {   
        IThongKeHoaDonService ThongKeHoaDon { get; }
    }
}