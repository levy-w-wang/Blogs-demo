using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace DemoFrame_Basic.Redis
{
    internal class RedisConnectionFactory
    {
        public string ConnectionString { get; set; }
        public string Password { get; set; }

        public ConnectionMultiplexer CurrentConnectionMultiplexer { get; set; }


        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <returns></returns>
        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <returns></returns>
        public void SetPassword(string password)
        {
            Password = password;
        }

        public ConnectionMultiplexer GetConnectionMultiplexer()
        {
            if (CurrentConnectionMultiplexer == null || !CurrentConnectionMultiplexer.IsConnected)
            {
                if (CurrentConnectionMultiplexer != null)
                {
                    CurrentConnectionMultiplexer.Dispose();
                }

                CurrentConnectionMultiplexer = GetConnectionMultiplexer(ConnectionString);
            }

            return CurrentConnectionMultiplexer;
        }


        private ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            ConnectionMultiplexer connectionMultiplexer;

            if (!string.IsNullOrWhiteSpace(Password) && !connectionString.ToLower().Contains("password"))
            {
                connectionString += $",password={Password}";
            }

            var redisConfiguration = ConfigurationOptions.Parse(connectionString);
            redisConfiguration.AbortOnConnectFail = true;
            redisConfiguration.AllowAdmin = false;
            redisConfiguration.ConnectRetry = 5;
            redisConfiguration.ConnectTimeout = 3000;
            redisConfiguration.DefaultDatabase = 0;
            redisConfiguration.KeepAlive = 20;
            redisConfiguration.SyncTimeout = 30 * 1000;
            redisConfiguration.Ssl = false;

            connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration);

            return connectionMultiplexer;
        }
    }
}
