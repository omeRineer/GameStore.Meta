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
        [BsonRepresentation(BsonType.String)]
        public Guid Client { get; set; }

        public string Type { get; set; }
        public string Level { get; set; }        
        public string Sender { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string? Content { get; set; }
        public List<string>? ReadBy { get; set; }

        public List<string>? Targets { get; set; }
        public List<string>? Topics { get; set; }

        public Dictionary<string, object>? Custom { get; set; }

    }
}
