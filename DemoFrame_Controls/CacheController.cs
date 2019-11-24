using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;
using DemoFrame_Basic.Caching;
using DemoFrame_CoreMvc;
using DemoFrame_CoreMvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DemoFrame_Controls
{
    public class CacheController : BaseController
    {
        /// <summary>
        /// 测试 MemoryCache 存储 获取
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        [HttpGet]
        [Route("mecache")]
        public ActionResult MeCache()
        {
            //测试
            var headersKeys = DemoWeb.HttpContext.Request.Headers.Keys;
            var token = DemoWeb.HttpContext.Request.Headers["token"];
            var sid = DemoWeb.HttpContext.Request.Headers["sid"];

            var key = "tkey";
            UserCache.Set(key, "测试数据");
            return Succeed(UserCache.Get(key));
        }

        /// <summary>
        /// 测试 redis 存储 获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("rediscache")/*,Obsolete*/]
        public ActionResult RedisCacheTest()
        {
            var key = "redisTest";
            RedisCache.Set(key, "测试123", new TimeSpan(0, 2, 0));
            return Succeed(RedisCache.Get<string>(key));
        }
    }
}
