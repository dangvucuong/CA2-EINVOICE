using Contracts.Repository.Base;
using Repository.Helper;

namespace Repository.Base
{
    public class CreateRepository<T> : BaseRepository, ICreateRepository<T>
    {
        public CreateRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<int> InsertAsync(T obj)
        {
            try
            {
                var tableName = typeof(T).Name;

                var param = DynamicParameterHelper.ConvertWithReturnParam(obj, "id");
                return _dbConnection.ExcuteScalarAsync(String.Format("{0}_insert", tableName), param, "id");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

