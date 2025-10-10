using AutoMapper;
using Core.ServiceModules;
using GameStore.Meta.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Business.Modules
{
    public class BusinessServiceModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<NotificationService>();
            services.AddScoped<ClientService>();
            services.AddScoped<SubscriberService>();
            services.AddScoped<ChannelService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
