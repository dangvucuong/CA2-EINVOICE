using Contracts.Service.Category;
using Model;
using Service.Base;

namespace Service.Category
{
    public class DonViCtsService : CRUDService<don_vi_cts>, IDonViCtsService
    {
        public DonViCtsService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.DonViCts;
        }

        public Task<IEnumerable<don_vi_cts>> SelectByDonViAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.Category.DonViCts.SelectByDonViAsync(donvi_ma_dv);
        }
    }
}