using Configuration;
using Core.ServiceModules;
using GameStore.Meta.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.DataAccess
{
    public class RepositoryServiceModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<NotificationRepository>();
            services.AddScoped<ClientRepository>();
            services.AddScoped<SubscriberRepository>();
            services.AddScoped<ChannelRepository>();

            services.AddSingleton<IMongoClient>(opt => new MongoClient(MetaConfiguration.MongoDbOptions.ConnectionString));
            services.AddScoped(opt =>
            {
                var client = opt.GetService<IMongoClient>();
                return client.GetDatabase(MetaConfiguration.MongoDbOptions.DataBase);
            });
        }
    }
}
