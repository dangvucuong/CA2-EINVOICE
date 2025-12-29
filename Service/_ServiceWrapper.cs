using Contract.Service;
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
using Contracts.Service.Pdf;
using Contracts.Service.RemoteSigning;
using Contracts.Service.TBSS;
using Contracts.Service.ThongKe;
using Contracts.Service.ToKhai;
using Contracts.Service.Upload;
using Contracts.Service.Xslt;
using Microsoft.AspNetCore.Http;
using Service.ApiSign;
using Service.BangTongHop;
using Service.Base;
using Service.Caching;
using Service.Category;
using Service.Contact;
using Service.Core;
using Service.HoaDon;
using Service.Notify;
using Service.Pdf;
using Service.RemoteSigning;
using Service.TBSS;
using Service.ThongKe;
using Service.ToKhai;
using Service.Upload;
using Service.User;
using Service.Xslt;

namespace Service
{
    public class ServiceWrapper : IServiceWrapper
    {
        private IServiceProvider _serviceProvider;
        private IBaseService _baseService;
        private ICoreSerivceWrapper _coreSerivceWrapper;
        private IUserServiceWrapper _userServiceWrapper;
        private ICategoryServiceWrapper _categoryServiceWrapper;
        private IUploadService _uploadService;
        private ICacheService _cacheService;
        private IContactSerivceWrapper _contactSerivceWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private INotifyServiceWrapper _notifyServiceWrapper;
        private IToKhaiSerivceWrapper _toKhaiSerivceWrapper;
        private IHoaDonServiceWrapper _hoaDonServiceWrapper;
        private IThongKeServiceWrapper _thongKeServiceWrapper;
        private IXsltService _xsltService;
        private IThongBaoSaiSotServiceWrapper _thongBaoSaiSotServiceWrapper;
        private IBangTongHopDuLieuServiceWrapper _bangTongHopDuLieuServiceWrapper;
        private IRemoteSigningSerivce _remoteSigningSerivce;
        private IPdfService _pdfService;
        private IApiSignHoaDonService _apiSignHoaDonService;
     
        public ServiceWrapper(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, ITaskQueueService taskQueueService, IPdfService pdfService)
        {
            this._serviceProvider = serviceProvider;
            this._httpContextAccessor = httpContextAccessor;
            this._pdfService = pdfService;
           
        }


        public IBaseService BaseService => _baseService ??= new BaseService(_serviceProvider);

        public ICoreSerivceWrapper Core => _coreSerivceWrapper ??= new CoreServiceWrapper(_serviceProvider);

        public IUserServiceWrapper User => _userServiceWrapper ??= new UserServiceWrapper(_serviceProvider);

        public ICategoryServiceWrapper Category => _categoryServiceWrapper ??= new CategoryServiceWrapper(_serviceProvider);


        public IUploadService Upload => _uploadService ??= new UploadService(_serviceProvider);

        public ICacheService Cache => _cacheService ??= new RedisCacheService();


        IHttpContextAccessor IServiceWrapper._httpContextAccessor => _httpContextAccessor;

        public IContactSerivceWrapper Contact => _contactSerivceWrapper ??= new ContactSerivceWrapper(_serviceProvider);

        public INotifyServiceWrapper Notify => _notifyServiceWrapper ??= new NotifyServiceWrapper(_serviceProvider);

        public IToKhaiSerivceWrapper ToKhaiSerivceWrapper => _toKhaiSerivceWrapper ??= new ToKhaiSerivceWrapper(_serviceProvider);

        public IHoaDonServiceWrapper HoaDon => _hoaDonServiceWrapper ??= new HoaDonServiceWrapper(_serviceProvider);

        public IXsltService Xslt => _xsltService ??= new XsltService(_serviceProvider);

        public IThongKeServiceWrapper ThongKe => _thongKeServiceWrapper ??= new ThongKeServiceWrapper(_serviceProvider);

        public IThongBaoSaiSotServiceWrapper ThongBaoSaiSot => _thongBaoSaiSotServiceWrapper ??= new ThongBaoSaiSotServiceWrapper(_serviceProvider);

        public IBangTongHopDuLieuServiceWrapper BangTongHopDuLieu => _bangTongHopDuLieuServiceWrapper ??= new BangTongHopDuLieuServiceWrapper(_serviceProvider);

        public IRemoteSigningSerivce RemoteSigningSerivce => _remoteSigningSerivce ??= new RemoteSigningSerivce(_serviceProvider);

        public IPdfService Pdf => _pdfService;

        public IApiSignHoaDonService ApiSignHoaDon => _apiSignHoaDonService ??= new ApiSignHoaDonService(_serviceProvider);

       
    }
}

