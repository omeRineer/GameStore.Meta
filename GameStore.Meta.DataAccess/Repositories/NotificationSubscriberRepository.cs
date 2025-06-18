using Core.DataAccess.Mongo;
using GameStore.Meta.Entities.Objects;
using MongoDB.Driver;

namespace GameStore.Meta.DataAccess.Repositories
{
    public class NotificationSubscriberRepository : MongoRepositoryBase<NotificationSubscriber, Guid>
    {
        public NotificationSubscriberRepository(IMongoDatabase dataBase) : base(dataBase, "NotificationSubscribers")
        {
        }
    }
}
