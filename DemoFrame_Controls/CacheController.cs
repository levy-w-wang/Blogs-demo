using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic.Caching;
using DemoFrame_CoreMvc;
using DemoFrame_CoreMvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    public class CacheController : BaseController
    {
        [HttpGet]
        [Route("mecache")]
        public ActionResult MeCache()
        {
            var key = "tkey";
            UserCache.Set(key, "测试数据");
            return Succeed(UserCache.Get(key));
        }

        [HttpGet]
        [Route("rediscache")]
        public ActionResult RedisCacheTest()
        {
            var key = "redisTest";
            RedisCache.Set(key, "测试123", new TimeSpan(0, 2, 0));
            return Succeed(RedisCache.Get<string>(key));
        }
    }
}
