using Contracts.Service.Category;
using Model.FuncResult;
using Model.Request.Base;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class DaiLyService : CRUDService<dai_ly>, IDaiLyService
    {
        public DaiLyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.DaiLy;
        }

        public Task<PagingResult<IEnumerable<dai_ly>>> SelectByDonViAsync(string donvi_ma_dv, PagingRequest pagingRequest)
        {
            return _repositoryWrapper.Category.DaiLy.SelectByDonViAsync(donvi_ma_dv, pagingRequest);
        }

        public Task<IEnumerable<dai_ly>> SelectByDonViHaveEmailAsync(string donvi_ma_dv)
        {
            return _repositoryWrapper.Category.DaiLy.SelectByDonViHaveEmailAsync(donvi_ma_dv);
        }
    }
}