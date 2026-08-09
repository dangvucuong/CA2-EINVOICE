

using System.Data;
using System.Data.SqlClient;
using Contracts.Repository.Base;
using Dapper;
using Model.Static;

namespace Repository.Base
{
    public class MSSQLConnection : IMSSQLConnection
    {

        private string _connectionName;
        public MSSQLConnection(string connectionName)
        {
            this._connectionName = connectionName;
        }
        private IDbConnection GetConnection()
        {
            var connectionString = AppSettings.DbConnections[_connectionName];
            IDbConnection _db = new SqlConnection(connectionString);
            return _db;
        }


        public async Task<bool> ExcuteQuerryAsync(string Querry)
        {
            await GetConnection().ExecuteAsync(Querry, null, commandType: CommandType.Text);
            return true;

        }


        public async Task<int> ExcuteScalarAsync(string StoreProcedureName, DynamicParameters param, string returnValueParamName)
        {
            await GetConnection().ExecuteScalarAsync(StoreProcedureName, param, commandType: CommandType.StoredProcedure);
            return param.Get<int>(returnValueParamName);
        }

        public async Task<bool> ExecuteAsync(string StoreProcedureName, DynamicParameters param)
        {
            await GetConnection().ExecuteAsync(StoreProcedureName, param, commandType: CommandType.StoredProcedure);
            return true;
        }

        public Task<IEnumerable<T>> SelectAsync<T>(string StoreProcedueName)
        {
            return SqlMapper.QueryAsync<T>(GetConnection(), StoreProcedueName, null, commandType: System.Data.CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> SelectAsync<T>(string StoreProcedueName, DynamicParameters param)
        {
            var result = SqlMapper.QueryAsync<T>(GetConnection(), StoreProcedueName, param, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public Task<T> SelectFirstOrDefaultAsync<T>(string StoreProcedueName)
        {
            return SqlMapper.QueryFirstOrDefaultAsync<T>(GetConnection(), StoreProcedueName, null, commandType: System.Data.CommandType.StoredProcedure);
        }

        public Task<T> SelectFirstOrDefaultAsync<T>(string StoreProcedueName, DynamicParameters param)
        {
            return SqlMapper.QueryFirstOrDefaultAsync<T>(GetConnection(), StoreProcedueName, param, commandType: System.Data.CommandType.StoredProcedure);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, DynamicParameters param)
        {
            return SqlMapper.QueryFirstOrDefaultAsync<T>(GetConnection(), sql, param, commandType: System.Data.CommandType.Text);
        }

    }
}

