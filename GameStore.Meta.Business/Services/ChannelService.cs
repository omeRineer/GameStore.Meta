using Core.Utilities.ResultTool;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Business.Services
{
    public class ChannelService
    {
        readonly ChannelRepository channelRepository;

        public ChannelService(ChannelRepository channelRepository)
        {
            this.channelRepository = channelRepository;
        }

        public async Task<IDataResult<CreateChannelResponse>> CreateChannelAsync(CreateChannelRequest model)
        {
            var isAvaible = await channelRepository.GetSingleOrDefaultAsync(f => f.Name == model.Name);
            if (isAvaible != null)
                return new ErrorDataResult<CreateChannelResponse>("Channel is avaible. Please, set a difference name value.");

            var channel = new Channel
            {
                Name = model.Name,
                Topics = model.Topics,
            };

            await channelRepository.AddAsync(channel);

            var result = new CreateChannelResponse(channel.Id, channel.Name, channel.Topics);

            return new SuccessDataResult<CreateChannelResponse>(result);
        }

    }
}
