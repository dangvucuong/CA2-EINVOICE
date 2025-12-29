using Contracts.Service.Category;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class DonViMuaChuKySoService : CRUDService<donvi_mua_chukyso>, IDonViMuaChuKySoService
    {
        public DonViMuaChuKySoService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.DonViMuaChuKySo;
        }

        public Task<IEnumerable<donvi_mua_chukyso>> SelectByDonViAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.Category.DonViMuaChuKySo.SelectByDonViAsync(donvi_ma_dv);
        }
    }
}