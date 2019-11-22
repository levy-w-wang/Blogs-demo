using DemoFrame_Basic.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace DemoFrame_Basic
{
    public class DemoWeb
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 当前请求HttpContext
        /// </summary>
        public static HttpContext HttpContext
        {
            get => _httpContextAccessor.HttpContext;
            set => _httpContextAccessor.HttpContext = value;
        }


        /// <summary>
        /// IocManager
        /// </summary>
        public static IIocManager IocManager { get; set; }

        /// <summary>
        /// Environment
        /// </summary>
        public static IHostingEnvironment Environment { get; set; }

        /// <summary>
        /// Configuration
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// MemoryCache
        /// </summary>
        public static IMemoryCache MemoryCache { get; set; }

        /// <summary>
        /// 获取当前请求客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim();
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}
