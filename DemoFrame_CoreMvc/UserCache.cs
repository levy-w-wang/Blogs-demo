using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;
using DemoFrame_Basic.Caching;
using DemoFrame_Basic.Extensions;
using DemoFrame_Models.CusEntitys;

namespace DemoFrame_CoreMvc
{
    public class UserCache
    {
        private static readonly MemoryCache Cache = new MemoryCache("User");

        private static TimeSpan _timeout = TimeSpan.Zero;
        private static TimeSpan Timeout
        {
            get
            {
                if (_timeout != TimeSpan.Zero)
                    return _timeout;
                try
                {
                    _timeout = TimeSpan.FromMinutes(20);
                    return _timeout;
                }
                catch (Exception)
                {
                    return TimeSpan.FromMinutes(10);
                }
            }
        }
        public static void Set(string key,string cache)
        {
            if (string.IsNullOrEmpty(cache))
                return;
            Cache.Set(key, cache, Timeout);
        }


        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(string);

            return Cache.Get<string>(key);
        }
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        internal static UserDto GetCurrentUser()
        {
            var key = DemoWeb.HttpContext.Request.Headers["sid"];
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            var user = Get(key);
            if (string.IsNullOrEmpty(user))
            {
                return null;
            }
            return user.ToNetType<UserDto>();
        }
    }
}
