using GameStore.Meta.Business.Services;
using GameStore.Meta.Models.Rest.NotificationSubscriber;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Meta.Controllers
{
    public class NotificationSubscribersController : BaseController
    {
        readonly NotificationSubscriberService NotificationSubscriberService;

        public NotificationSubscribersController(NotificationSubscriberService notificationSubscriberService)
        {
            NotificationSubscriberService = notificationSubscriberService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateNotificationSubscriberModel model)
        {
            var result = await NotificationSubscriberService.CreateSubscriberAsync(model);

            return Ok(result);
        }
    }
}
