using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace DemoFrame_Basic.Caching
{
    /// <summary>
    /// MemoryCache缓存
    /// </summary>
    public class MemoryCache
    {
        private static readonly HashSet<string> Keys = new HashSet<string>();

        /// <summary>
        /// 缓存前缀
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prefix"></param>
        public MemoryCache(string prefix)
        {
            Prefix = prefix + "_";
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return DemoWeb.MemoryCache.Get<T>(Prefix + key);
        }

        /// <summary>
        /// 设置 无过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Set(string key, object data)
        {
            key = Prefix + key;
            DemoWeb.MemoryCache.Set(key, data);
            if (!Keys.Contains(key))
            {
                Keys.Add(key);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpiration"></param>
        public void Set(string key, object data, DateTimeOffset absoluteExpiration)
        {
            key = Prefix + key;
            DemoWeb.MemoryCache.Set(key, data, absoluteExpiration);
            if (!Keys.Contains(key))
            {
                Keys.Add(key);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        public void Set(string key, object data, TimeSpan absoluteExpirationRelativeToNow)
        {
            key = Prefix + key;
            DemoWeb.MemoryCache.Set(key, data, absoluteExpirationRelativeToNow);
            if (!Keys.Contains(key))
            {
                Keys.Add(key);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expirationToken"></param>
        public void Set(string key, object data, IChangeToken expirationToken)
        {
            key = Prefix + key;
            DemoWeb.MemoryCache.Set(key, data, expirationToken);
            if (!Keys.Contains(key))
            {
                Keys.Add(key);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        public void Set(string key, object data, MemoryCacheEntryOptions options)
        {
            key = Prefix + key;
            DemoWeb.MemoryCache.Set(key, data, options);
            if (!Keys.Contains(key))
            {
                Keys.Add(key);
            }
        }

        /// <summary>
        /// 移除某个
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            key = Prefix + key;
            DemoWeb.MemoryCache.Remove(key);
            if (Keys.Contains(key))
            {
                Keys.Remove(key);
            }
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public void ClearAll()
        {
            foreach (var key in Keys)
            {
                DemoWeb.MemoryCache.Remove(key);
            }
            Keys.Clear();
        }

    }
}
