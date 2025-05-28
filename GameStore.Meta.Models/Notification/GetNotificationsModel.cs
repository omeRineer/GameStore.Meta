using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Notification
{
    public class GetNotificationsModel
    {
        public List<GetNotifications_Item>? Notifications { get; set; }
    }

    public class GetNotifications_Item
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
