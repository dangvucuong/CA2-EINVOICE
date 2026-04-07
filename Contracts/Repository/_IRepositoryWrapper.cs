
using Contract.Repository.Category;
using Contract.Repository.Core;
using Contract.Repository.User;
using Contracts.Repository.BangTongHop;
using Contracts.Repository.Base;
using Contracts.Repository.Contact;
using Contracts.Repository.HoaDon;
using Contracts.Repository.TBSS;
using Contracts.Repository.ToKhai;

namespace Contracts.Repository
{
    public interface IRepositoryWrapper : IBaseRepositoryWrapper
    {
        ICoreRepositoryWrapper Core { get; }
        IUserRepositoryWrapper User { get; }
        ICategoryRepositoryWrapper Category { get; }
        IContactRepositoryWrapper Contact { get; }
        IToKhaiRepositoryWrapper ToKhaiWrapper { get; }
        IHoaDonRepositoryWrapper HoaDon { get; }
        IThongBaoSaiSotRepositoryWrapper ThongBaoSaiSot { get; }
        IBangTongHopDuLieuRepositoryWrapper BangTongHopDuLieu {get;}
    }
}

