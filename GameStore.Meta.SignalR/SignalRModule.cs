using Core.ServiceModules;
using GameStore.Meta.SignalR.Hubs;
using GameStore.Meta.SignalR.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.SignalR
{
    public class SignalRModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<HubConnectionManager<NotificationHub>>();
        }
    }
}
