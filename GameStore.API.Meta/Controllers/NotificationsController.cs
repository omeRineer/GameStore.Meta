using GameStore.Meta.Business.Services;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest.Notification;
using MeArch.Module.Security.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace GameStore.API.Meta.Controllers
{
    public class NotificationsController : BaseController
    {
        readonly NotificationService NotificationService;
        public NotificationsController(NotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        [HttpPost("Send")]
        [Authorize("SuperAdmin,API.Meta.Notifications.Send")]
        public async Task<IActionResult> SendAsync([FromBody] CreateNotificationModel request)
        {
            var result = await NotificationService.SendAsync(request);

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotificationsAsync()
        {
            var result = await NotificationService.GetListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await NotificationService.GetDetailAsync(id);

            return Ok(result);
        }
    }
}
