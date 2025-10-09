using GameStore.Meta.Business.Services;
using GameStore.Meta.Models.Rest;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Meta.Controllers
{
    public class SubscribersController : BaseController
    {
        readonly SubscriberService NotificationSubscriberService;

        public SubscribersController(SubscriberService notificationSubscriberService)
        {
            NotificationSubscriberService = notificationSubscriberService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSubscriberRequest model)
        {
            var result = await NotificationSubscriberService.CreateSubscriberAsync(model);

            return Ok(result);
        }
    }
}
