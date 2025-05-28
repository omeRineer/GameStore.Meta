using Core.Entities.Concrete.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Entities.Objects
{
    public class Notification : MongoBaseEntity<Guid>
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Guid>? ReadBy { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Guid>? TargetUsers { get; set; }

    }
}
