using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Subscriber
{
    public class CreateSubscriberRequest
    {
        public Guid Client { get; set; }
        public string Key { get; set; }
        public List<string>? Topics { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
