using Core.DataAccess.Mongo;
using GameStore.Meta.Entities.Objects;
using MongoDB.Driver;

namespace GameStore.Meta.DataAccess.Repositories
{
    public class ChannelRepository : MongoRepositoryBase<Channel, Guid>
    {
        public ChannelRepository(IMongoDatabase dataBase) : base(dataBase, "Channels")
        {
        }
    }
}
