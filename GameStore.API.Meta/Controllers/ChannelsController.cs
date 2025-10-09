using GameStore.Meta.Business.Services;
using GameStore.Meta.Models.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Meta.Controllers
{
    public class ChannelsController : BaseController
    {
        readonly ChannelService channelService;

        public ChannelsController(ChannelService channelService)
        {
            this.channelService = channelService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateChannelRequest model)
        {
            var result = await channelService.CreateChannelAsync(model);

            return Response(result);
        }
    }
}
