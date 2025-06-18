using Core.Utilities.ResultTool;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest.Client;

namespace GameStore.Meta.Business.Services
{
    public class ClientService
    {
        readonly ClientRepository ClientRepository;

        public ClientService(ClientRepository clientRepository)
        {
            ClientRepository = clientRepository;
        }

        public async Task<IResult> CreateClientAsync(CreateClientModel model)
        {
            var isAvaible = await ClientRepository.GetSingleOrDefaultAsync(f => f.Signature == model.Signature);
            if (isAvaible != null)
                return new ErrorResult("İstemci mevcut. Farklı bir Signature girin.");

            var client = new Client
            {
                Signature = model.Signature,
                Name = model.Name,
                ExpireDate = model.ExpireDate,
            };

            await ClientRepository.AddAsync(client);

            return new SuccessResult("İstemci oluşturuldu.");
        }

        public async Task<IDataResult<GetClientDetailModel>> GetDetailAsync(Guid id)
        {
            var client = await ClientRepository.GetSingleOrDefaultAsync(f => f.Id == id);

            if (client == null)
                return new ErrorDataResult<GetClientDetailModel>("İstemci bulunamadı");

            var result = new GetClientDetailModel
            {
                Id = client.Id,
                Name = client.Name,
                ExpireDate = client.ExpireDate,
                CreateDate = client.CreateDate,
                Signature = client.Signature
            };

            return new SuccessDataResult<GetClientDetailModel>(result);
        }
    }
}
