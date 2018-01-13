using StackExchange.Redis;
using System;

namespace HWL.Redis.Client
{
    public class RedisBase
    {
        public int DbNum { get; set; }
        private readonly ConnectionMultiplexer _conn;

        public RedisBase(int dbNum = 0)
            : this(dbNum, null)
        {
        }

        public RedisBase(int dbNum, string readWriteHosts)
        {
            DbNum = dbNum;
            _conn = string.IsNullOrWhiteSpace(readWriteHosts) ? StackExchangeRedisManager.Instance : StackExchangeRedisManager.GetConnectionMultiplexer(readWriteHosts);
        }

        public T Exec<T>(Func<IDatabase, T> func)
        {
            var database = _conn.GetDatabase(DbNum);
            return func(database);
        }

        public void Exec(Action<IDatabase> func)
        {
            var database = _conn.GetDatabase(DbNum);
            func(database);
        }

        public void SelectDb(ref IDatabase dataBase)
        {
            dataBase = _conn.GetDatabase(DbNum);
        }

        //private string ConvertJson<T>(T value)
        //{
        //    string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
        //    return result;
        //}

        //private T ConvertObj<T>(RedisValue value)
        //{
        //    return JsonConvert.DeserializeObject<T>(value);
        //}

        //private List<T> ConvetList<T>(RedisValue[] values)
        //{
        //    List<T> result = new List<T>();
        //    foreach (var item in values)
        //    {
        //        var model = ConvertObj<T>(item);
        //        result.Add(model);
        //    }
        //    return result;
        //}

        //private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        //{
        //    return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        //}

    }

}