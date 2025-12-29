using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Dapper;
using Model.Table;
using Repository.Base;

namespace Repository.Category
{
    public class CoQuanThueRespository : CRUDRepository<co_quan_thue>, ICoQuanThueRespository
    {
        public CoQuanThueRespository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<co_quan_thue> SelectByMaAsync(string ma_cqt)
        {
            var param = new DynamicParameters();
            param.Add("@ma_cqt", ma_cqt);
            return _dbConnection.SelectFirstOrDefaultAsync<co_quan_thue>("co_quan_thue_select_by_ma",param);
        }
    }
}