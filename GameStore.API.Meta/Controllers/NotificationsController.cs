using GameStore.Meta.Business.Services;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Message;
using GameStore.Meta.Models.Rest.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GameStore.API.Meta.Controllers
{
    [Authorize(AuthenticationSchemes = "NotificationApiKeyScheme")]
    public class NotificationsController : BaseController
    {
        readonly NotificationService NotificationService;
        public NotificationsController(NotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendAsync([FromBody] PushNotificationModel request)
        {
            var result = await NotificationService.PushAsync(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsAsync()
        {
            var result = await NotificationService.GetListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await NotificationService.GetDetailAsync(id);

            return Ok(result);
        }
    }
}
