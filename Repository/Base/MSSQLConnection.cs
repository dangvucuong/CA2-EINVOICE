

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

        private SqlConnection CreateConnection()
        {
            var connectionString = AppSettings.DbConnections[_connectionName];
            return new SqlConnection(connectionString);
        }


        public async Task<bool> ExcuteQuerryAsync(string Querry)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(Querry, null, commandType: CommandType.Text);
            return true;

        }


        public async Task<int> ExcuteScalarAsync(string StoreProcedureName, DynamicParameters param, string returnValueParamName)
        {
            using var connection = CreateConnection();
            await connection.ExecuteScalarAsync(StoreProcedureName, param, commandType: CommandType.StoredProcedure);
            return param.Get<int>(returnValueParamName);
        }

        public async Task<bool> ExecuteAsync(string StoreProcedureName, DynamicParameters param)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(StoreProcedureName, param, commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string StoreProcedueName)
        {
            using var connection = CreateConnection();
            return await SqlMapper.QueryAsync<T>(connection, StoreProcedueName, null, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> SelectAsync<T>(string StoreProcedueName, DynamicParameters param)
        {
            using var connection = CreateConnection();
            return await SqlMapper.QueryAsync<T>(connection, StoreProcedueName, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> SelectFirstOrDefaultAsync<T>(string StoreProcedueName)
        {
            using var connection = CreateConnection();
            return await SqlMapper.QueryFirstOrDefaultAsync<T>(connection, StoreProcedueName, null, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> SelectFirstOrDefaultAsync<T>(string StoreProcedueName, DynamicParameters param)
        {
            using var connection = CreateConnection();
            return await SqlMapper.QueryFirstOrDefaultAsync<T>(connection, StoreProcedueName, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, DynamicParameters param)
        {
            using var connection = CreateConnection();
            return await SqlMapper.QueryFirstOrDefaultAsync<T>(connection, sql, param, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<T>> SelectByQueryAsync<T>(string sql, DynamicParameters param)
        {
            using var connection = CreateConnection();
            return await SqlMapper.QueryAsync<T>(connection, sql, param, commandType: CommandType.Text);
        }

    }
}

