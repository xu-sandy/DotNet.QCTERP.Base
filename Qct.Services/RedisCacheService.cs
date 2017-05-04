using Qct.Infrastructure.Data.Configuration;
using Qct.Infrastructure.Json;
using Qct.ISevices;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly string RedisConnnectionString;
        private readonly string name = "RedisConnectstring";
        public RedisCacheService()
        {
            RedisConnnectionString = ConnectionStringConfig.GetConnectionString(name);
        }

        public T GetObject<T>(string key, bool isDelayed = false, TimeSpan? expire = null)
        {
            var db = GetDatabase();
            var objectText = db.StringGet(key);
            var obj = JsonHelper.ToObject<T>(objectText);
            if (expire != null && expire > new TimeSpan(0))
            {
                db.KeyExpire(key, expire);
            }
            return obj;
        }

        public void RemoveObject<T>(string key)
        {
            var db = GetDatabase();
            var objectText = db.KeyDelete(key);
        }

        public void SetObject<T>(string key, T obj, TimeSpan? expire = null)
        {
            var jsonText = JsonHelper.ToJson(obj);
            var db = GetDatabase();
            if (expire != null && expire > new TimeSpan(0))
            {
                db.StringSet(key, jsonText, expire);
            }
            else
            {
                db.StringSet(key, jsonText);
            }
        }

        private ConnectionMultiplexer GetConnection()
        {
            return ConnectionMultiplexer.Connect(RedisConnnectionString);
        }

        private IDatabase GetDatabase()
        {
            return GetConnection().GetDatabase();
        }
    }
}
