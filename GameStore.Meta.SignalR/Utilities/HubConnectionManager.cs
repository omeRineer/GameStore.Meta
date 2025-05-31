using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Meta.SignalR.Utilities
{
    public class HubConnectionManager<THub>
    {
        Dictionary<string, List<string>> Connections { get; }
        public HubConnectionManager()
        {
            Connections = new();
        }

        public void AddConnection(string key, string connectionId)
        {
            lock (Connections)
            {
                if (!Connections.ContainsKey(key))
                    Connections[key] = new List<string>();

                Connections[key].Add(connectionId);
            }
        }

        public void RemoveConnection(string key, string connectionId)
        {
            lock (Connections)
            {
                if (Connections.ContainsKey(key))
                {
                    Connections[key].Remove(connectionId);

                    if (Connections[key].Count == 0)
                        Connections.Remove(key);
                }
            }
        }

        public IEnumerable<KeyValuePair<string, List<string>>> GetConnections()
        {
            lock (Connections)
            {
                return Connections.AsEnumerable();
            }
        }
    }
}
