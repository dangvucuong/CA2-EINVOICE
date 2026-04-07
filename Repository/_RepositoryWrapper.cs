using Contract.Repository.Category;
using Contract.Repository.Core;
using Contract.Repository.User;
using Contracts.Repository;
using Contracts.Repository.BangTongHop;
using Contracts.Repository.Base;
using Contracts.Repository.Contact;
using Contracts.Repository.HoaDon;
using Contracts.Repository.TBSS;
using Contracts.Repository.ToKhai;
using Repository.BangTongHop;
using Repository.Base;
using Repository.Category;
using Repository.Contact;
using Repository.Core;
using Repository.HoaDon;
using Repository.TBSS;
using Repository.ToKhai;
using Repository.User;

namespace Repository
{
    public class RepositoryWrapper : BaseRepositoryWrapper, IRepositoryWrapper
    {
        private ICoreRepositoryWrapper _coreRepositoryWrapper;
        private IUserRepositoryWrapper _userRepositoryWrapper;
        private ICategoryRepositoryWrapper _categoryRepositoryWrapper;
        private IContactRepositoryWrapper _contactRepositoryWrapper;
        private IToKhaiRepositoryWrapper _toKhaiRepositoryWrapper;
        private IHoaDonRepositoryWrapper _hoaDonRepositoryWrapper;
        private IThongBaoSaiSotRepositoryWrapper _thongBaoSaiSotRepositoryWrapper;
        private IBangTongHopDuLieuRepositoryWrapper _bangTongHopDuLieuRepositoryWrapper;
        public RepositoryWrapper(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
        }

        public ICoreRepositoryWrapper Core => _coreRepositoryWrapper ??= new CoreRepositoryWrapper(_connectionStrings);

        public IUserRepositoryWrapper User => _userRepositoryWrapper ??= new UserRepositoryWrapper(_connectionStrings);

        public ICategoryRepositoryWrapper Category => _categoryRepositoryWrapper ??= new CategoryRepositoryWrapper(_connectionStrings);

        public IContactRepositoryWrapper Contact => _contactRepositoryWrapper ??= new ContactRepositoryWrapper(_connectionStrings);

        public IToKhaiRepositoryWrapper ToKhaiWrapper => _toKhaiRepositoryWrapper ??= new ToKhaiRepositoryWrapper(_connectionStrings);

        public IHoaDonRepositoryWrapper HoaDon => _hoaDonRepositoryWrapper ??= new HoaDonRepositoryWrapper(_connectionStrings);

        public IThongBaoSaiSotRepositoryWrapper ThongBaoSaiSot => _thongBaoSaiSotRepositoryWrapper ??= new ThongBaoSaiSotRepositoryWrapper(_connectionStrings);

        public IBangTongHopDuLieuRepositoryWrapper BangTongHopDuLieu => _bangTongHopDuLieuRepositoryWrapper??= new BangTongHopDuLieuRepositoryWrapper(_connectionStrings);
    }
}

