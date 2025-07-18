using Configuration;
using GameStore.Meta.Configuration.Options;
using RabbitMQ.Client;

namespace GameStore.API.Meta.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static async Task<IServiceCollection> AddRabbitMqAsync(this IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = MetaConfiguration.RabbitMqOptions.Host;
            connectionFactory.Port = MetaConfiguration.RabbitMqOptions.Port;
            connectionFactory.VirtualHost = MetaConfiguration.RabbitMqOptions.VirtualHost;
            connectionFactory.UserName = MetaConfiguration.RabbitMqOptions.UserName;
            connectionFactory.Password = MetaConfiguration.RabbitMqOptions.Password;

            IConnection? connection = null;
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(1000);
                connection = await connectionFactory.CreateConnectionAsync();

                if (connection != null && connection.IsOpen)
                    break;
            }
            var channel = await connection.CreateChannelAsync();

            services.AddSingleton<IConnection>(connection);
            services.AddSingleton<IChannel>(channel);

            return services;
        }
    }

}
