using Contract.Service.Category;
using Contract.Service.Core;
using Contract.Service.User;
using Contracts.Service.ApiSign;
using Contracts.Service.BangTongHop;
using Contracts.Service.Base;
using Contracts.Service.Cache;
using Contracts.Service.Contact;
using Contracts.Service.Core;
using Contracts.Service.HoaDon;
using Contracts.Service.Notify;
using Contracts.Service.RemoteSigning;
using Contracts.Service.TBSS;
using Contracts.Service.ThongKe;
using Contracts.Service.ToKhai;
using Contracts.Service.Upload;
using Contracts.Service.Xslt;
using Microsoft.AspNetCore.Http;

namespace Contract.Service
{
    public interface IServiceWrapper
    {
        IBaseService BaseService { get; }
        ICoreSerivceWrapper Core { get; }
        IUserServiceWrapper User { get; }
        ICategoryServiceWrapper Category { get; }
        IUploadService Upload { get; }
        ICacheService Cache { get; }
        IHttpContextAccessor _httpContextAccessor { get; }
        IContactSerivceWrapper Contact { get; }
        INotifyServiceWrapper Notify { get; }
        IToKhaiSerivceWrapper ToKhaiSerivceWrapper { get; }
        IHoaDonServiceWrapper HoaDon { get; }
        IXsltService Xslt { get; }
        // IPdfService Pdf { get; }
        IThongKeServiceWrapper ThongKe { get; }
        IThongBaoSaiSotServiceWrapper ThongBaoSaiSot { get; }
        IBangTongHopDuLieuServiceWrapper BangTongHopDuLieu { get; }
        IRemoteSigningSerivce RemoteSigningSerivce { get; }
        IApiSignHoaDonService ApiSignHoaDon {get;}
       
    }
}

