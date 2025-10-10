using AutoMapper;
using Core.Utilities.ResultTool;
using GameStore.Meta.Business.Helpers;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest;
using GameStore.Meta.Models.Rest.Subscriber;

namespace GameStore.Meta.Business.Services
{
    public class SubscriberService
    {
        readonly SubscriberRepository SubscriberRepository;
        readonly ClientRepository ClientRepository;
        readonly IMapper Mapper;

        public SubscriberService(SubscriberRepository subscriberRepository, ClientRepository clientRepository, IMapper mapper)
        {
            SubscriberRepository = subscriberRepository;
            ClientRepository = clientRepository;
            Mapper = mapper;
        }

        public async Task<IDataResult<CreateSubscriberResponse>> CreateAsync(CreateSubscriberRequest model)
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

            var result = Mapper.Map<CreateSubscriberResponse>(subscriber);

            return new SuccessDataResult<CreateSubscriberResponse>(result, "Subscriber is created.");
        }

        public async Task<IDataResult<UpdateSubscriberResponse>> UpdateAsync(UpdateSubscriberRequest model)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.Id == model.Id);
            if (subscriber == null)
                return new ErrorDataResult<UpdateSubscriberResponse>("Subscriber is not found.");

            subscriber = Mapper.Map(model, subscriber);

            await SubscriberRepository.UpdateAsync(subscriber);

            var result = Mapper.Map<UpdateSubscriberResponse>(subscriber);

            return new SuccessDataResult<UpdateSubscriberResponse>(result, "Subscriber is updated.");
        }

        public async Task<IDataResult<GetSubscriberDetailResponse>> GetDetailAsync(Guid id)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f=> f.Id == id);

            if (subscriber == null)
                return new ErrorDataResult<GetSubscriberDetailResponse>("Subscriber is not avaible.");

            var result = Mapper.Map<GetSubscriberDetailResponse>(subscriber);

            return new SuccessDataResult<GetSubscriberDetailResponse>(result);
        }

        public async Task<IDataResult<GetSubscriberDetailResponse?>> GetDetailAsync(string apiKey)
        {
            var subscriber = await SubscriberRepository.GetSingleOrDefaultAsync(f => f.ApiKey == apiKey);

            if(subscriber == null)
                return new ErrorDataResult<GetSubscriberDetailResponse?>("Subscriber is not avaible.");

            var result = Mapper.Map<GetSubscriberDetailResponse>(subscriber);

            return new SuccessDataResult<GetSubscriberDetailResponse?>(result);
        }
    }
}
