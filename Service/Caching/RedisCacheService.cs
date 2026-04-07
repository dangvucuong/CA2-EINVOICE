using Contracts.Service.Cache;
using Model.Static;
using StackExchange.Redis;
using Common;
namespace Service.Caching
{
    public class RedisCacheService : ICacheService
    {
        private static readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var config = ConfigurationOptions.Parse(AppSettings.RedisConfig.Host);
            config.SyncTimeout = 15000; // tăng timeout lên 15 giây (tùy bạn chọn)
            config.ConnectTimeout = 5000; // (optional) timeout khi connect
            config.AbortOnConnectFail = false; // (optional) không throw nếu kết nối thất bại lần đầu

            return ConnectionMultiplexer.Connect(config);
            // return ConnectionMultiplexer.Connect(AppSettings.RedisConfig.Host);
        });
        public static ConnectionMultiplexer redis => lazyConnection.Value;

        public T GetData<T>(string key)
        {
            IDatabase db = redis.GetDatabase();
            var value = db.StringGet(key);
            if (value.HasValue)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
            return default(T);
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            IDatabase db = redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            if (value.HasValue)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
            return default(T);

        }

        public async Task<IEnumerable<string>> GetKeysAsync(string pattern)
        {
            // using (var redis = ConnectionMultiplexer.Connect(AppSettings.RedisConfig.Host))
            // {
            IDatabase db = redis.GetDatabase();
            var keys = redis.GetServer($"{AppSettings.RedisConfig.Host}:{AppSettings.RedisConfig.Port}").Keys(db.Database, pattern).ToArray();
            return keys.Select(x => x.ToString()).ToList();
            // }
        }

        public async Task<IEnumerable<T>> GetListDataAsync<T>(string pattern)
        {
            try
            {
                // //cần lưu ý nếu triển khai nhiều server redis cùng lúc
                // using (var redis = ConnectionMultiplexer.Connect(AppSettings.RedisConfig.Host))
                // {
                IDatabase db = redis.GetDatabase();
                var keys = redis.GetServer($"{AppSettings.RedisConfig.Host}:{AppSettings.RedisConfig.Port}").Keys(db.Database, pattern).ToArray();
                var values = await db.StringGetAsync(keys);
                var result = values.Where(x => x.HasValue).Select(x => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(x));
                return result;
                // }
            }
            catch (System.Exception ex)
            {
                return null;
            }

        }
        public async Task<bool> RemoveDataAsync(string key)
        {
            IDatabase db = redis.GetDatabase();
            var result = await db.KeyDeleteAsync(key);
            return result;
        }

        public async Task<bool> RemoveDataAsync(List<string> keys)
        {
            // using (var redis = ConnectionMultiplexer.Connect(AppSettings.RedisConfig.Host))
            // {
            //cần lưu ý nếu triển khai nhiều server redis cùng lúc
            IDatabase db = redis.GetDatabase();
            var cacheTasks = new List<Task>();
            foreach (var key in keys)
            {
                cacheTasks.Add(db.KeyDeleteAsync(key));
            }
            await Task.WhenAll(cacheTasks);
            return true;
            // }

        }

        public async Task<bool> RemoveDataByPatternAsync(string pattern)
        {
            using (var redis = ConnectionMultiplexer.Connect(AppSettings.RedisConfig.Host))
            {
                //cần lưu ý nếu triển khai nhiều server redis cùng lúc
                IDatabase db = redis.GetDatabase();
                var keys = redis.GetServer($"{AppSettings.RedisConfig.Host}:{AppSettings.RedisConfig.Port}").Keys(db.Database, pattern).ToArray();
                var result = await db.KeyDeleteAsync(keys);
                return true;
            }
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset? expirationTime)
        {
            try
            {
                IDatabase db = redis.GetDatabase();
                TimeSpan? timeSpan = null;
                if (expirationTime != null)
                {
                    timeSpan = expirationTime - DateTime.Now;
                }
                var result = await db.StringSetAsync(key, Newtonsoft.Json.JsonConvert.SerializeObject(value), timeSpan);
                return result;
            }
            catch (System.Exception ex)
            {
                // throw ex;
                return false;
            }
        }

        public async Task<bool> SetDictionaryDataAsync(string keyPrefix, IDictionary<string, string> dictionary, DateTimeOffset? expirationTime)
        {

            IDatabase db = redis.GetDatabase();
            var cacheTasks = new List<Task>();
            foreach (var item in dictionary)
            {
                if (item.Value != null)
                {
                    var id = item.Key;
                    if (id != string.Empty)
                    {
                        cacheTasks.Add(this.SetDataAsync($"{keyPrefix}:{id}", item.Value, expirationTime));
                    }
                }
            }
            await Task.WhenAll(cacheTasks);
            return true;

        }

        public async Task<bool> SetListDataAsync<T>(string keyPrefix, string itemKeyField, IEnumerable<T> values, DateTimeOffset? expirationTime)
        {

            IDatabase db = redis.GetDatabase();
            var cacheTasks = new List<Task>();
            foreach (var value in values)
            {
                if (value != null)
                {
                    var id = value.GetPropValue(itemKeyField)?.ToString() ?? "";
                    if (id != string.Empty)
                    {
                        cacheTasks.Add(this.SetDataAsync($"{keyPrefix}:{id}", value, expirationTime));
                        // await this.SetDataAsync($"{keyPrefix}:{id}", value, expirationTime);
                    }
                }
            }
            await Task.WhenAll(cacheTasks);
            return true;

        }
    }
}