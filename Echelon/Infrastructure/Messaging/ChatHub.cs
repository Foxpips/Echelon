using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Echelon.Infrastructure.Messaging
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> Dic = new ConcurrentDictionary<string, string>();

        public void Send(string name, string message, string uniqueId)
        {
            Clients.All.SendMessage(name, message, uniqueId);
        }

        public void SendToSpecific(string name, string message, string to)
        {
            Clients.Caller.SendMessage(name, message);
            Clients.Client(Dic[to]).SendMessage(name, message);
        }

        public void Notify(string name, string id)
        {
            if (Dic.ContainsKey(name))
            {
                Clients.Caller.ChangeName();
            }
            else
            {
                Dic.TryAdd(name, id);

                foreach (var entry in Dic)
                {
                    Clients.Caller.Online(entry.Key);
                }

                Clients.Others.Enters(name);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string s;
            var name = Dic.FirstOrDefault(x => x.Value == Context.ConnectionId.ToString());
            Dic.TryRemove(name.Key, out s);
            return Clients.All.Disconnected(name.Key);
        }
    }
}