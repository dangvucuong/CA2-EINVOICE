using Contracts.Service.HoaDon;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class LoaiHoaDonService : CRUDServiceWithCache<loai_hoa_don>, ILoaiHoaDonService
    {
        public LoaiHoaDonService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.HoaDon.LoaiHoaDon;
        }

        protected override void ConfigKey()
        {
            this._itemKeyField = "id";
            this._keyPrefix = "loai_hoa_don:";
        }
    }
}