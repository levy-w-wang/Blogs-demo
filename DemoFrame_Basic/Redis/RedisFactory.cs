using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic.Extensions;

namespace DemoFrame_Basic.Redis
{
    public static class RedisFactory
    {
        private static readonly object Locker = new object();

        private static RedisConnectionFactory factory;

        private static void InitRedisConnection()
        {
            try
            {
                factory = new RedisConnectionFactory();
                var connectionString = DemoWeb.Configuration["Redis:ConnectionString"];
#if DEBUG
                connectionString = "127.0.0.1:6379";
#endif
                factory.ConnectionString = connectionString;
                factory.Password = DemoWeb.Configuration["Redis:Pwd"];

            }
            catch (Exception e)
            {
                LogHelper.Logger.Fatal(e, "Redis连接创建失败。");
            }
        }

        public static RedisClient GetClient()
        {
            //先判断一轮,减少锁,提高效率
            if (factory == null || string.IsNullOrEmpty(factory.ConnectionString))
            {
                //防止并发创建
                lock (Locker)
                {
                    InitRedisConnection();
                }
            }

            return new RedisClient(factory.GetConnectionMultiplexer())
            {
                DefaultDatabase = DemoWeb.Configuration["Redis:DefaultDatabase"].ToInt()
            };

        }
    }
}
