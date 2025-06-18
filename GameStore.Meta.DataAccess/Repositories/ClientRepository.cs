using Core.DataAccess.Mongo;
using GameStore.Meta.Entities.Objects;
using MongoDB.Driver;

namespace GameStore.Meta.DataAccess.Repositories
{
    public class ClientRepository : MongoRepositoryBase<Client, Guid>
    {
        public ClientRepository(IMongoDatabase dataBase) : base(dataBase, "Clients")
        {
        }
    }
}
