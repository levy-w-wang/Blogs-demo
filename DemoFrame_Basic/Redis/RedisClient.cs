using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic.Extensions;
using StackExchange.Redis;

namespace DemoFrame_Basic.Redis
{
    /// <summary>
    /// Redis Client
    /// </summary>
    public class RedisClient : IDisposable
    {
        public int DefaultDatabase { get; set; } = 0;

        private readonly ConnectionMultiplexer _client;
        private IDatabase _db;

        public RedisClient(ConnectionMultiplexer client)
        {
            _client = client;
            UseDatabase();
        }

        public void UseDatabase(int db = -1)
        {
            if (db == -1)
                db = DefaultDatabase;
            _db = _client.GetDatabase(db);
        }


        public string StringGet(string key)
        {
            return _db.StringGet(key).ToString();
        }


        public void StringSet(string key, string data)
        {
            _db.StringSet(key, data);
        }

        public void StringSet(string key, string data, TimeSpan timeout)
        {
            _db.StringSet(key, data, timeout);
        }


        public T Get<T>(string key)
        {
            var json = StringGet(key);
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            return json.ToNetType<T>();
        }

        public void Set(string key, object data)
        {
            var json = data.ToJson();
            _db.StringSet(key, json);
        }

        public void Set(string key, object data, TimeSpan timeout)
        {
            var json = data.ToJson();
            _db.StringSet(key, json, timeout);
        }

        /// <summary>
        /// Exist
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist(string key)
        {
            return _db.KeyExists(key);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            return _db.KeyDelete(key);
        }

        /// <summary>
        /// Set Expire to Key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool Expire(string key, TimeSpan? expiry)
        {
            return _db.KeyExpire(key, expiry);
        }

        /// <summary>
        /// 计数器  如果不存在则设置值，如果存在则添加值  如果key存在且类型不为long  则会异常
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">只有第一次设置有效期生效</param>
        /// <returns></returns>
        public long SetStringIncr(string key, long value = 1, TimeSpan? expiry = null)
        {
            var nubmer = _db.StringIncrement(key, value);
            if (nubmer == 1 && expiry != null)//只有第一次设置有效期（防止覆盖）
                _db.KeyExpireAsync(key, expiry);//设置有效期
            return nubmer;
        }

        /// <summary>
        /// 读取计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetStringIncr(string key)
        {
            var value = StringGet(key);
            return string.IsNullOrWhiteSpace(value) ? 0 : long.Parse(value);
        }

        /// <summary>
        /// 计数器-减少 如果不存在则设置值，如果存在则减少值  如果key存在且类型不为long  则会异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long StringDecrement(string key, long value = 1)
        {
            var nubmer = _db.StringDecrement(key, value);
            return nubmer;
        }



        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
