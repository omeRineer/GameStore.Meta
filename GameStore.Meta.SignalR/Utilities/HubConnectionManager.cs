using GameStore.Meta.SignalR.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.SignalR.Utilities
{
    public class HubConnectionManager<THub>
    {
        Dictionary<string, ConnectionUser> Connections { get; }
        public HubConnectionManager()
        {
            Connections = new();
        }

        public void AddConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                if (!Connections.ContainsKey(context.UserIdentifier))
                    Connections[context.UserIdentifier] = new ConnectionUser
                    {
                        ConnectionId = context.ConnectionId,
                        Id = context.UserIdentifier,
                        Topics = context.User.Claims.Where(f => f.Type == "Topic")
                                                    .Select(s => s.Value)
                                                    .ToList()
                    };
            }
        }

        public void RemoveConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                if (Connections.ContainsKey(context.UserIdentifier))
                    Connections.Remove(context.UserIdentifier);
            }
        }

        public IEnumerable<KeyValuePair<string, ConnectionUser>> GetTargets(List<string>? users = null,
                                                                            List<string>? topics = null)
        {
            lock (Connections)
            {
                List<KeyValuePair<string, ConnectionUser>> targetUsers = new();
                if (users != null)
                    targetUsers = Connections.Where(f => users.Contains(f.Key) && f.Value.Topics == null).ToList();

                List<KeyValuePair<string, ConnectionUser>> targetTopics = new();
                if (topics != null)
                    targetTopics = Connections.Where(f => f.Value.Topics != null && topics.Any(x => f.Value.Topics.Contains(x))).ToList();

                var resultTargets = targetUsers.Concat(targetTopics)
                                               .AsEnumerable();

                return resultTargets;
            }
            
        }
    }
}
