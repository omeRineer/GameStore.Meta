using Core.DataAccess.Mongo;
using Core.Entities.Abstract;
using GameStore.Meta.Entities.Objects;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace GameStore.Meta.DataAccess.Repositories
{
    public class ClientRepository : MongoRepositoryBase<Client, Guid>
    {
        public ClientRepository(IMongoDatabase dataBase) : base(dataBase, "Clients")
        {
        }

    }
}
