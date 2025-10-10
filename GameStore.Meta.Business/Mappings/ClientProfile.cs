using AutoMapper;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest.Client;

namespace GameStore.Meta.Business.Mappings
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientResponse>();
            CreateMap<Client, CreateClientResponse>().IncludeBase<Client, ClientResponse>();
            CreateMap<Client, UpdateClientResponse>().IncludeBase<Client, ClientResponse>();
            CreateMap<Client, GetClientDetailResponse>().IncludeBase<Client, ClientResponse>();

            CreateMap<CreateClientRequest, Client>();
            CreateMap<UpdateClientRequest, Client>();
        }
    }
}
