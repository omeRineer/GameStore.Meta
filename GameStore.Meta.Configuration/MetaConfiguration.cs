using GameStore.Meta.Configuration.Options;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;

namespace Configuration
{
    public static class MetaConfiguration
    {
        readonly static IConfigurationRoot Configuration;
        static MetaConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                                .Build();
        }

        public static MongoDbOptions MongoDbOptions { get => Configuration.GetSection("MongoDbOptions").Get<MongoDbOptions>(); }
        public static RabbitMqOptions RabbitMqOptions { get => Configuration.GetSection("RabbitMqOptions").Get<RabbitMqOptions>(); }
        public static TokenOptions TokenOptions { get => Configuration.GetSection("TokenOptions").Get<TokenOptions>(); }
        public static NotificationOptions NotificationOptions { get => Configuration.GetSection("NotificationOptions").Get<NotificationOptions>(); }

    }
}
