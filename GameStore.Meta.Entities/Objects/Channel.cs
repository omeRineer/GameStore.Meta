using Core.Entities.Concrete.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Entities.Objects
{
    public class Channel : MongoBaseEntity<Guid>
    {
        public string Name { get; set; }
        public List<string>? Topics { get; set; }
    }
}
