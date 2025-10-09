using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.SignalR.Model
{
    public class ConnectionUser
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public List<string>? Topics { get; set; }
    }
}
