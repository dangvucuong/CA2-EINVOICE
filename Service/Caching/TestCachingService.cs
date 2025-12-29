using System.Diagnostics;
using Contract.Service;
using Model.Table;
using StackExchange.Redis;

namespace Service.Caching
{
    public class TestCachingService
    {
        private readonly IServiceWrapper _serviceWrapper;
        public TestCachingService(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }
        public async Task Test2()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                IDatabase db = redis.GetDatabase();

                // var users = (await _serviceWrapper.User.User.SelectAllAsync()).ToList();

                // List<RedisKey> keys = new List<RedisKey>();
                // users.ForEach(item =>
                // {
                //     // db.StringSet("User:" + item.id.ToString(), JsonSerializer.Serialize(item));
                //     keys.Add($"User:{item.id}");
                // });
                // var test = db.StringGet("User:16299");

                // RedisValue[] values = db.StringGet(keys.ToArray());

                var keys2 = redis.GetServer("localhost:6379").Keys(db.Database, "User*").ToArray();
                var values2 = db.StringGet(keys2);
                var users = values2.Where(x=>x.HasValue).Select(x => Newtonsoft.Json.JsonConvert.DeserializeObject<user>(x)).ToList();
                // var users = new List<user>();
                // foreach (var value in values2)
                // {
                //     if (value.HasValue)
                //     {
                //         var xxxxx = JsonSerializer.Deserialize<user>(value);
                //         users.Add(xxxxx);
                //     }
                // }
                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;
                var stopwatch2 = new Stopwatch();
                stopwatch2.Start();
                // var count = users.Count;
                var users_db = (await _serviceWrapper.User.User.SelectAllAsync()).ToList();
                stopwatch2.Stop();
                TimeSpan elapsedTime2 = stopwatch2.Elapsed;
            }
            catch (System.Exception ex)
            {
                var m = ex.Message;
            }


        }

    }
}