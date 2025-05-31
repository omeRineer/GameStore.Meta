using Core.DataAccess.Mongo;
using Core.Entities.Abstract;
using Core.Entities.DTO.Pagination;
using GameStore.Meta.Entities.Objects;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.DataAccess.Repositories
{
    public class NotificationRepository : MongoRepositoryBase<Notification, Guid>
    {
        public NotificationRepository(IMongoDatabase dataBase) : base(dataBase, "Notifications")
        {
        }
    }
}
