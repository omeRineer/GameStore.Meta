using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Subscriber
{
    public class SubscriberResponse
    {
        public Guid Id { get; set; }
        public Guid Client { get; set; }
        public string Key { get; set; }
        public string ApiKey { get; set; }
        public List<string>? Topics { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
