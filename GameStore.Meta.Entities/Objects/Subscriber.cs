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
    public class Subscriber:MongoBaseEntity<Guid>
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Client { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Key { get; set; }
        public string ApiKey { get; set; }
        public List<string>? Topics { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
