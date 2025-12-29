using Contracts.Repository.Base;
using Dapper;

namespace Repository.Base
{
    public class DeleteRepository<T> : BaseRepository, IDeleteRepository<T>
    {
        public DeleteRepository(IMSSQLConnection dbConnection) : base(dbConnection)
        {
        }

        public Task<bool> DeleteAsync(int id, int userId)
        {
            try
            {
                var tableName = typeof(T).Name;

                var param = new DynamicParameters();
                param.Add("@id", id);
                param.Add("@user_id", userId);
                return _dbConnection.ExecuteAsync(String.Format("{0}_delete", tableName), param);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

