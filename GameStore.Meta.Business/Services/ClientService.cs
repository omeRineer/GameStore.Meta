using AutoMapper;
using Core.Utilities.ResultTool;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest;
using GameStore.Meta.Models.Rest.Client;

namespace GameStore.Meta.Business.Services
{
    public class ClientService
    {
        readonly ClientRepository ClientRepository;
        readonly IMapper Mapper;

        public ClientService(ClientRepository clientRepository, IMapper mapper)
        {
            ClientRepository = clientRepository;
            Mapper = mapper;
        }

        public async Task<IDataResult<CreateClientResponse>> CreateAsync(CreateClientRequest model)
        {
            var isAvaible = await ClientRepository.GetSingleOrDefaultAsync(f => f.Signature == model.Signature);
            if (isAvaible != null)
                return new ErrorDataResult<CreateClientResponse>("Client is avaible. Please, set a difference signature value.");

            var client = new Client
            {
                Signature = model.Signature,
                Name = model.Name,
                ExpireDate = model.ExpireDate,
            };

            await ClientRepository.AddAsync(client);

            var result = Mapper.Map<CreateClientResponse>(client);

            return new SuccessDataResult<CreateClientResponse>(result, "Client is created.");
        }

        public async Task<IDataResult<UpdateClientResponse>> UpdateAsync(UpdateClientRequest model)
        {
            var client = await ClientRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            if (client == null)
                return new ErrorDataResult<UpdateClientResponse>("Client is not found.");

            client = Mapper.Map(model, client);

            await ClientRepository.UpdateAsync(client);

            var result = Mapper.Map<UpdateClientResponse>(client);

            return new SuccessDataResult<UpdateClientResponse>(result, "Client is updated.");
        }

        public async Task<IDataResult<GetClientDetailResponse>> GetDetailAsync(Guid id)
        {
            var client = await ClientRepository.GetSingleOrDefaultAsync(f => f.Id == id);
            if (client == null)
                return new ErrorDataResult<GetClientDetailResponse>("Client is not found.");

            var result = Mapper.Map<GetClientDetailResponse>(client);

            return new SuccessDataResult<GetClientDetailResponse>(result);
        }
    }
}
