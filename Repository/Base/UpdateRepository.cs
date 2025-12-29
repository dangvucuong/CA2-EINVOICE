using Contracts.Repository.Base;
using Repository.Helper;

namespace Repository.Base
{
    public class UpdateRepository<T> : BaseRepository, IUpdateRepository<T>
    {
        public UpdateRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<bool> UpdateAsync(T obj)
        {
            try
            {
                var tableName = typeof(T).Name;

                var param = DynamicParameterHelper.ConvertWithOutCreatitonParams(obj);
                return _dbConnection.ExecuteAsync(String.Format("{0}_update", tableName), param);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

