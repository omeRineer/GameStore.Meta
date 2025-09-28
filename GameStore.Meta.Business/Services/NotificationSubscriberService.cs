using Core.Utilities.ResultTool;
using GameStore.Meta.Business.Helpers;
using GameStore.Meta.DataAccess.Repositories;
using GameStore.Meta.Entities.Objects;
using GameStore.Meta.Models.Rest.Client;
using GameStore.Meta.Models.Rest.NotificationSubscriber;

namespace GameStore.Meta.Business.Services
{
    public class NotificationSubscriberService
    {
        readonly NotificationSubscriberRepository NotificationSubscriberRepository;
        readonly ClientRepository ClientRepository;

        public NotificationSubscriberService(NotificationSubscriberRepository notificationSubscriberRepository, ClientRepository clientRepository)
        {
            NotificationSubscriberRepository = notificationSubscriberRepository;
            ClientRepository = clientRepository;
        }

        public async Task<IDataResult<CreateNotificationSubscriberResponseModel>> CreateSubscriberAsync(CreateNotificationSubscriberModel model)
        {
            var client = await ClientRepository.GetSingleOrDefaultAsync(f => f.Id == model.Client);
            if (client == null)
                return new ErrorDataResult<CreateNotificationSubscriberResponseModel>("Client is not avaible.");

            var avaibleSubscriber = await NotificationSubscriberRepository.GetSingleOrDefaultAsync(f => f.Key == model.Key && f.Client == model.Client);
            if (avaibleSubscriber != null)
                return new ErrorDataResult<CreateNotificationSubscriberResponseModel>("Subscriber is avaible. Please, set a different key");

            string apiKey = CryptoHelper.Encrypt(model.Key, client.Signature);
            var subscriber = new NotificationSubscriber
            {
                Client = model.Client,
                Key = model.Key,
                ApiKey = apiKey,
                ExpireDate = model.ExpireDate,
            };

            await NotificationSubscriberRepository.AddAsync(subscriber);

            var result = new CreateNotificationSubscriberResponseModel
            {
                ApiKey = apiKey,
                ExpireDate = model.ExpireDate,
                Client = client.Name
            };

            return new SuccessDataResult<CreateNotificationSubscriberResponseModel>(result, "Subscriber is created.");
        }

        public async Task<IDataResult<NotificationSubscriber?>> GetSubscriberAsync(string apiKey)
        {
            var subscriber = await NotificationSubscriberRepository.GetSingleOrDefaultAsync(f=> f.ApiKey == apiKey);

            return new SuccessDataResult<NotificationSubscriber?>(subscriber);
        }
    }
}
