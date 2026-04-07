using Contracts.Service.HoaDon;
using Model.Table;
using Service.Base;

namespace Service.HoaDon
{
    public class LoaiHoaDonCTService : CRUDServiceWithCache<loai_hoa_don_ct>, ILoaiHoaDonCTService
    {
        public LoaiHoaDonCTService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase =_repositoryWrapper.HoaDon.LoaiHoaDonCT;
        }

        protected override void ConfigKey()
        {
            this._itemKeyField="id";
            this._keyPrefix="loai_hoa_don_ct:";
        }
    }
}