using Contracts.Repository.Base;
using Contracts.Repository.Category;
using Dapper;
using Model.Respone.Category;
using Repository.Base;

namespace Repository.Category
{
    public class DiaBanHanhChinhRepository : BaseRepository, IDiaBanHanhChinhRepository
    {
        public DiaBanHanhChinhRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<DiaBanHanhChinh> SelectByMaDiaBanAsync(string maDiaBan)
        {
            var param = new DynamicParameters();
            param.Add("@_MA_DBHC", maDiaBan);
            return _dbConnection.SelectFirstOrDefaultAsync<DiaBanHanhChinh>("usp_Timdiabanhanhchinh", param);
        }
    }
}