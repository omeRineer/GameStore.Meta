using Core.Utilities.ServiceTools;
using GameStore.API.Meta.Workers.Main;
using GameStore.Meta.Business.Services;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Message;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GameStore.API.Meta.Workers
{
    public class NotificationWorker : ConsumerWorker
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationWorker(IChannel channel, IConnection connection, IServiceScopeFactory scopeFactory)
            : base(channel, connection, "Notification_Queue", "Notification_Worker", false)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();

            var notification = GetMessage<PushNotificationModel>(args);
            await notificationService.PushAsync(notification);
        }
    }
}
