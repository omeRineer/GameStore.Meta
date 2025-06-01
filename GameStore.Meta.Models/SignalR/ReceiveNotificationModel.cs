using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.Models.SignalR
{
    public class ReceiveNotificationModel
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateDate { get; set; }
        public Dictionary<string, object>? Custom { get; set; }
    }
}
