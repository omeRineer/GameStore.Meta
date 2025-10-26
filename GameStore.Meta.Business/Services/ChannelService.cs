using AutoMapper;
using Core.Utilities.ResultTool;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest;
using GameStore.Meta.Models.Rest.Channel;
using GameStore.Meta.Models.Rest.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Business.Services
{
    public class ChannelService
    {
        readonly ChannelRepository channelRepository;
        readonly IMapper Mapper;

        public ChannelService(ChannelRepository channelRepository, IMapper mapper)
        {
            this.channelRepository = channelRepository;
            Mapper = mapper;
        }

        public async Task<IDataResult<CreateChannelResponse>> CreateAsync(CreateChannelRequest model)
        {
            var isAvaible = await channelRepository.GetSingleOrDefaultAsync(f => f.Prefix == model.Prefix);
            if (isAvaible != null)
                return new ErrorDataResult<CreateChannelResponse>("Channel is avaible. Please, set a difference name value.");

            var channel = Mapper.Map<Channel>(model);
            channel.Topics = channel.Topics?.Select(s=> $"{model.Prefix}_{s}").ToList();

            await channelRepository.AddAsync(channel);

            var result = Mapper.Map<CreateChannelResponse>(channel);

            return new SuccessDataResult<CreateChannelResponse>(result, "Channel is created.");
        }

        public async Task<IDataResult<UpdateChannelResponse>> UpdateAsync(UpdateChannelRequest model)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            if (channel == null)
                return new ErrorDataResult<UpdateChannelResponse>("Channel is not found.");

            channel = Mapper.Map(model, channel);
            channel.Topics = channel.Topics?.Select(s => $"{channel.Prefix}{s}").ToList();

            await channelRepository.UpdateAsync(channel);

            var result = Mapper.Map<UpdateChannelResponse>(channel);

            return new SuccessDataResult<UpdateChannelResponse>(result, "Channel is updated.");
        }

        public async Task<IDataResult<GetChannelDetailResponse>> GetDetailAsync(Guid id)
        {
            var channel = await channelRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            if (channel == null)
                return new ErrorDataResult<GetChannelDetailResponse>("Channel is not found.");

            var result = Mapper.Map<GetChannelDetailResponse>(channel);

            return new SuccessDataResult<GetChannelDetailResponse>(result);
        }

    }
}
