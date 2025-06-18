using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.NotificationSubscriber
{
    public class CreateNotificationSubscriberModel
    {
        public Guid Client { get; set; }
        public string Key { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
