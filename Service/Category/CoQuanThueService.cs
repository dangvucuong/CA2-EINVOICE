using Contracts.Service.Category;
using Dapper;
using Model.Table;
using Service.Base;

namespace Service.Category
{
    public class CoQuanThueService : CRUDService<co_quan_thue>, ICoQuanThueService
    {
        public CoQuanThueService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._repositoryBase = _repositoryWrapper.Category.CoQuanThue;
        }

        public Task<co_quan_thue> SelectByMaAsync(string ma_cqt)
        {
            return _repositoryWrapper.Category.CoQuanThue.SelectByMaAsync(ma_cqt);
           
        }
    }
}