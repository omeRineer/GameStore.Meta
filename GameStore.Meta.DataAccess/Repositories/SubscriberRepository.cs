using Core.DataAccess.Mongo;
using GameStore.Meta.Entities.Objects;
using MongoDB.Driver;

namespace GameStore.Meta.DataAccess.Repositories
{
    public class SubscriberRepository : MongoRepositoryBase<Subscriber, Guid>
    {
        public SubscriberRepository(IMongoDatabase dataBase) : base(dataBase, "Subscribers")
        {
        }
    }
}
