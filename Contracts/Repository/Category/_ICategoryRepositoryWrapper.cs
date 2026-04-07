using Contracts.Repository.Base;
using Contracts.Repository.Category;

namespace Contract.Repository.Category
{
    public interface ICategoryRepositoryWrapper : IBaseRepositoryWrapper
    {
        IDonViRepository DonVi { get; }
        IDonViMuaChuKySoRepository DonViMuaChuKySo { get; }
        IHangHoaRepository HangHoa { get; }
        IKhachHangRepository KhachHang { get; }
        ICoQuanThueRespository CoQuanThue { get; }
        IWatermarkTemplateRepository WatermarkTemplate { get; }
        IDiaBanHanhChinhRepository DiaBanHanhChinh { get; }
        IDaiLyRepository DaiLy { get; }
        IDonViCtsRepository DonViCts { get; }

    }
}

