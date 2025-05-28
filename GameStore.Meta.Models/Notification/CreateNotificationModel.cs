using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.Notification
{
    public class CreateNotificationModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public List<Guid>? ReadBy { get; set; }
        public List<Guid>? TargetUsers { get; set; }
    }
}
