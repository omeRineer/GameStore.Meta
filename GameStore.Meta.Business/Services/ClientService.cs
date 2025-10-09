using Core.Utilities.ResultTool;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest;

namespace GameStore.Meta.Business.Services
{
    public class ClientService
    {
        readonly ClientRepository ClientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            ClientRepository = clientRepository;
        }

        public async Task<IDataResult<CreateClientResponse>> CreateClientAsync(CreateClientRequest model)
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

            var result = new CreateClientResponse(client.Signature, client.Name, client.ExpireDate, client.CreateDate);

            return new SuccessDataResult<CreateClientResponse>(result);
        }

        public async Task<IDataResult<GetClientDetailResponse>> GetDetailAsync(Guid id)
        {
            var client = await ClientRepository.GetSingleOrDefaultAsync(f => f.Id == id);

            if (client == null)
                return new ErrorDataResult<GetClientDetailResponse>("Client is not found.");

            var result = new GetClientDetailResponse(client.Id, client.Signature, client.Name, client.ExpireDate, client.CreateDate);

            return new SuccessDataResult<GetClientDetailResponse>(result);
        }
    }
}
