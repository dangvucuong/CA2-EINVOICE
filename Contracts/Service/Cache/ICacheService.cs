namespace Contracts.Service.Cache
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string key);
        T GetData<T>(string key);
        Task<IEnumerable<T>> GetListDataAsync<T>(string pattern);

        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset? expirationTime);
        Task<bool> SetListDataAsync<T>(string keyPrefix, string itemKeyField, IEnumerable<T> values, DateTimeOffset? expirationTime);
        Task<bool> SetDictionaryDataAsync(string keyPrefix, IDictionary<string, string> dictionary, DateTimeOffset? expirationTime);

        Task<bool> RemoveDataAsync(string key);
        Task<bool> RemoveDataAsync(List<string> keys);
        Task<bool> RemoveDataByPatternAsync(string pattern);
        Task<IEnumerable<string>> GetKeysAsync(string pattern);
    }
}