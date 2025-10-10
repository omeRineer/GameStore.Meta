using GameStore.Meta.Business.Services;
using GameStore.Meta.Models.Rest;
using GameStore.Meta.Models.Rest.Subscriber;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Meta.Controllers
{
    public class SubscribersController : BaseController
    {
        readonly SubscriberService SubscriberService;

        public SubscribersController(SubscriberService subscriberService)
        {
            SubscriberService = subscriberService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await SubscriberService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSubscriberRequest model)
        {
            var result = await SubscriberService.CreateAsync(model);

            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSubscriberRequest model)
        {
            var result = await SubscriberService.UpdateAsync(model);

            return Ok(result);
        }
    }
}
