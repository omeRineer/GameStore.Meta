using Core.Utilities.ResultTool;
using GameStore.Meta.Business.Helpers;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest;

namespace GameStore.Meta.Business.Services
{
    public class SubscriberService
    {
        readonly SubscriberRepository SubscriberRepository;
        readonly ClientRepository ClientRepository;

        public SubscriberService(SubscriberRepository subscriberRepository, ClientRepository clientRepository)
        {
            SubscriberRepository = subscriberRepository;
            ClientRepository = clientRepository;
        }

        public async Task<IDataResult<CreateSubscriberResponse>> CreateSubscriberAsync(CreateSubscriberRequest model)
        {
            var client = await ClientRepository.GetSingleOrDefaultAsync(f => f.Id == model.Client);
            if (client == null)
                return new ErrorDataResult<CreateSubscriberResponse>("Client is not avaible.");

            var avaibleSubscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.Client == model.Client);
            if (avaibleSubscriber != null)
                return new ErrorDataResult<CreateSubscriberResponse>("Subscriber is avaible. Please, set a different key");

            string apiKey = CryptoHelper.Encrypt(model.Key, client.Signature);
            var subscriber = new Subscriber
            {
                Client = model.Client,
                Key = model.Key,
                ApiKey = apiKey,
                ExpireDate = model.ExpireDate,
            };

            await SubscriberRepository.AddAsync(subscriber);

            var result = new CreateSubscriberResponse(client.Id, subscriber.ApiKey, subscriber.ExpireDate);

            return new SuccessDataResult<CreateSubscriberResponse>(result, "Subscriber is created.");
        }

        public async Task<IDataResult<GetSubscriberResponse>> GetDetailAsync(Guid id)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f=> f.Id == id);

            if (subscriber == null)
                return new ErrorDataResult<GetSubscriberResponse>("Subscriber is not avaible.");

            var result = new GetSubscriberResponse
                (
                    subscriber.Id,
                    subscriber.Client,
                    subscriber.Key,
                    subscriber.ApiKey,
                    subscriber.Topics,
                    subscriber.ExpireDate,
                    subscriber.CreateDate
                );

            return new SuccessDataResult<GetSubscriberResponse>(result);
        }

        public async Task<IDataResult<SubscriberItem?>> GetSubscriberByApiKey(string apiKey)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.ApiKey == apiKey);

            if(subscriber == null)
                return new ErrorDataResult<SubscriberItem?>();

            var result = new SubscriberItem
                (
                    subscriber.Id,
                    subscriber.Client,
                    subscriber.Key,
                    subscriber.ApiKey,
                    subscriber.Topics,
                    subscriber.ExpireDate
                );

            return new SuccessDataResult<SubscriberItem?>(result);
        }
    }
}
