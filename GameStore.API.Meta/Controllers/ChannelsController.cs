using GameStore.Meta.Business.Services;
using GameStore.Meta.Models.Rest;
using GameStore.Meta.Models.Rest.Channel;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await channelService.GetDetailAsync(id);

            return Response(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateChannelRequest model)
        {
            var result = await channelService.CreateAsync(model);

            return Response(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateChannelRequest model)
        {
            var result = await channelService.UpdateAsync(model);

            return Response(result);
        }
    }
}
