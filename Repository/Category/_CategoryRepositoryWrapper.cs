using Contract.Repository.Category;
using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Repository.Base;

namespace Repository.Category
{
    public class CategoryRepositoryWrapper : BaseRepositoryWrapper, ICategoryRepositoryWrapper
    {
        private IDonViRepository _donViRepository;
        private IHangHoaRepository _hangHoaRepository;
        private IKhachHangRepository _khachHangRepository;
        private IDonViMuaChuKySoRepository _donViMuaChuKySoRepository;
        private ICoQuanThueRespository _coQuanThue;
        private IWatermarkTemplateRepository _watermarkTemplateRepository;
        private IDiaBanHanhChinhRepository _diaBanHanhChinhRepository;
        private IDaiLyRepository _daiLyRepository;
        private IDonViCtsRepository _donViCtsRepository;
        public CategoryRepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public IDonViRepository DonVi => _donViRepository ??= new DonViRepository(_defaultConnection);

        public IHangHoaRepository HangHoa => _hangHoaRepository ??= new HangHoaRepository(_defaultConnection);

        public IKhachHangRepository KhachHang => _khachHangRepository ??= new KhachHangRepository(_defaultConnection);

        public IDonViMuaChuKySoRepository DonViMuaChuKySo => _donViMuaChuKySoRepository ??= new DonViMuaChuKySoRepository(_defaultConnection);

        public ICoQuanThueRespository CoQuanThue => _coQuanThue ??= new CoQuanThueRespository(_defaultConnection);

        public IWatermarkTemplateRepository WatermarkTemplate => _watermarkTemplateRepository ??= new WatermarkTemplateRepository(_defaultConnection);

        public IDiaBanHanhChinhRepository DiaBanHanhChinh => _diaBanHanhChinhRepository ??= new DiaBanHanhChinhRepository(_defaultConnection);

        public IDaiLyRepository DaiLy => _daiLyRepository ??= new DaiLyRepository(_defaultConnection);

        public IDonViCtsRepository DonViCts => _donViCtsRepository ??=new DonViCtsRepository(_defaultConnection);
    }
}

