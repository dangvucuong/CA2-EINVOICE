using Dapper;

namespace Contracts.Repository.Base
{
    public interface IMSSQLConnection
    {
        Task<bool> ExecuteAsync(string StoreProcedureName, DynamicParameters param);
        Task<bool> ExcuteQuerryAsync(string Querry);
        Task<int> ExcuteScalarAsync(string StoreProcedureName, DynamicParameters param, string returnValueParamName);
        Task<IEnumerable<T>> SelectAsync<T>(string StoreProcedueName);
        Task<IEnumerable<T>> SelectAsync<T>(string StoreProcedueName, DynamicParameters param);
        Task<T> SelectFirstOrDefaultAsync<T>(string StoreProcedueName);
        Task<T> SelectFirstOrDefaultAsync<T>(string StoreProcedueName, DynamicParameters param);
    }
}

