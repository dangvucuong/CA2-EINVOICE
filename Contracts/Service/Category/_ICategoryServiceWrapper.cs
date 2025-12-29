using Contracts.Service.Base;
using Contracts.Service.Category;

namespace Contract.Service.Category
{
    public interface ICategoryServiceWrapper : IBaseService
    {
        IDonViService DonVi { get; }
        IDonViMuaChuKySoService DonViMuaChuKySo { get; }
        IKhachHangService KhachHang { get; }
        IHangHoaService HangHoa { get; }
        ICoQuanThueService CoQuanThue { get; }
        IWatermarkService Watermark { get; }
        IDaiLyService DaiLy { get; }
        IDonViCtsService DonViCts { get; }
    }
}

