using AutoMapper;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest.Client;
using GameStore.Meta.Models.Rest.Subscriber;

namespace GameStore.Meta.Business.Mappings
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<Subscriber, SubscriberResponse>();
            CreateMap<Subscriber, CreateSubscriberResponse>().IncludeBase<Subscriber, SubscriberResponse>();
            CreateMap<Subscriber, UpdateSubscriberResponse>().IncludeBase<Subscriber, SubscriberResponse>();

            CreateMap<CreateSubscriberRequest, Subscriber>();
            CreateMap<UpdateSubscriberRequest, Subscriber>();
        }
    }
}
