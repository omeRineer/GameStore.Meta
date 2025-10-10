using AutoMapper;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest.Channel;
using GameStore.Meta.Models.Rest.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Business.Mappings
{
    public class ChannelProfile:Profile
    {
        public ChannelProfile()
        {
            CreateMap<Channel, ChannelResponse>();
            CreateMap<Channel, CreateChannelResponse>().IncludeBase<Channel, ChannelResponse>();
            CreateMap<Channel, UpdateChannelResponse>().IncludeBase<Channel, ChannelResponse>();
            CreateMap<Channel, GetChannelDetailResponse>().IncludeBase<Channel, ChannelResponse>();

            CreateMap<CreateChannelRequest, Channel>();
            CreateMap<UpdateChannelRequest, Channel>();
        }
    }
}
