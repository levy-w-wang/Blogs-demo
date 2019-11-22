using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic.Extensions;
using DemoFrame_Basic.Redis;

namespace DemoFrame_Basic.Caching
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisCache
    {
        private static RedisClient _client;
        private static RedisClient Client => _client ?? (_client = RedisFactory.GetClient());

        private static string ToKey(string key)
        {
            return $"Cache_redis_{key}";
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            try
            {
                var redisKey = ToKey(key);
                return Client.Get<T>(redisKey);
            }
            catch (Exception e)
            {
                LogHelper.Logger.Fatal(e, "RedisCache.Get \n key:{0}", key);
                return default(T);
            }
        }

        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static T TryGet<T>(string key, out bool result)
        {
            result = true;
            try
            {
                var redisKey = ToKey(key);
                return Client.Get<T>(redisKey);
            }
            catch (Exception e)
            {
                LogHelper.Logger.Fatal(e, "RedisCache.TryGet \n key:{0}", key);
                result = false;
                return default(T);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="setFunc"></param>
        /// <param name="expiry"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> setFunc, TimeSpan? expiry = null)
        {
            var redisKey = ToKey(key);
            var result = TryGet<T>(redisKey, out var success);
            if (success && result == null)
            {
                result = setFunc();
                try
                {
                    Set(redisKey, result, expiry);
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Fatal(e, "RedisCache.Get<T> \n key:{0}", key);
                }
            }
            return result;
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var allRedisKey = ToKey("||Keys||");
            var redisKey = ToKey(key);

            var allkeyRedisValue = Client.StringGet(allRedisKey);
            var keys = allkeyRedisValue.ToNetType<List<string>>() ?? new List<string>();
            if (!keys.Contains(redisKey))
            {
                keys.Add(redisKey);
                Client.Set(allRedisKey, keys);
            }
            if (expiry.HasValue)
            {
                Client.StringSet(redisKey, value.ToJson(), expiry.Value);
            }
            else
            {
                Client.StringSet(redisKey, value.ToJson());
            }

            return true;
        }

        /// <summary>
        /// 重新设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        public static void ResetItemTimeout(string key, TimeSpan expiry)
        {
            var redisKey = ToKey(key);
            Client.Expire(redisKey, expiry);
        }

        /// <summary>
        /// Exist
        /// </summary>
        /// <param name="key">原始key</param>
        /// <returns></returns>
        public static bool Exist(string key)
        {
            var redisKey = ToKey(key);
            return Client.Exist(redisKey);
        }

        /// <summary>
        /// 计数器 增加  能设置过期时间的都设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool SetStringIncr(string key, long value = 1, TimeSpan? expiry = null, bool needRest0 = false)
        {
            var redisKey = ToKey(key);
            try
            {
                if (expiry.HasValue)
                {
                    if (Exist(key) && needRest0)
                    {
                        var exitValue = GetStringIncr(key);
                        Client.SetStringIncr(redisKey, value - exitValue, expiry.Value);
                    }
                    else
                    {
                        Client.SetStringIncr(redisKey, value, expiry.Value);
                    }
                }
                else
                {
                    if (Exist(key) && needRest0)
                    {
                        var exitValue = GetStringIncr(key);
                        Client.SetStringIncr(redisKey, value - exitValue);
                    }
                    else
                    {
                        Client.SetStringIncr(redisKey, value);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Fatal($"计数器-增加错误，原因：{e.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetStringIncr(string key)
        {
            var redisKey = ToKey(key);
            return Client.GetStringIncr(redisKey);
        }

        /// <summary>
        /// 计数器 - 减少
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool StringDecrement(string key, long value = 1)
        {
            var redisKey = ToKey(key);
            try
            {
                Client.StringDecrement(redisKey, value);
                return true;
            }
            catch (Exception e)
            {
                LogHelper.Logger.Fatal($"计数器-减少错误，原因：{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            var redisKey = ToKey(key);
            return Client.Delete(redisKey);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public static void Clear()
        {
            //因为codis不支持keys之类的命令，所以只能自己记录下来，然后通过这个来清理。
            var redisKey = ToKey("||Keys||");

            var keys = Client.Get<List<string>>(redisKey);
            var notExists = new List<string>();
            foreach (var key in keys)
            {
                if (Client.Exist(key))
                    Client.Delete(key);
                else
                    notExists.Add(key);
            }
            if (notExists.Count > 0)
            {
                keys.RemoveAll(s => notExists.Contains(s));
                Client.Set(redisKey, keys);
            }
        }
    }
}
