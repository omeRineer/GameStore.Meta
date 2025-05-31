using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Rest.Notification
{
    public class GetNotificationsModel
    {
        public List<GetNotifications_Item>? Notifications { get; set; }
    }

    public class GetNotifications_Item
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
        public Guid Sender { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
