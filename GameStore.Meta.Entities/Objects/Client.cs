using Core.Entities.Concrete.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Entities.Objects
{
    public class Client : MongoBaseEntity<Guid>
    {
        public string Signature { get; set; }
        public string Name { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
