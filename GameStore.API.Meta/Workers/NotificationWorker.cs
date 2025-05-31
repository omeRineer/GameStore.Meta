using GameStore.API.Meta.Workers.Main;
using GameStore.Meta.Business.Services;
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
        readonly NotificationService NotificationService;

        public NotificationWorker(IChannel channel, NotificationService notificationService) : base(channel, 
                                                                                                    "Notification_Queue",
                                                                                                    "Notification_Worker")
        {
            NotificationService = notificationService;
        }

        protected override async Task HandleAsync(object model, BasicDeliverEventArgs args)
        {
            var notification = GetMessage<PushNotificationModel>(args);
            var createResult = await NotificationService.PushAsync(notification);
        }
    }
}
