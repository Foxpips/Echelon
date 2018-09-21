using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Echelon.Infrastructure.Messaging
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ChatUser> Dic =
            new ConcurrentDictionary<string, ChatUser>();

        public void Send(ChatUser user, string message)
        {
            Clients.All.SendMessage(user, message);
        }

        public void SendToSpecific(ChatUser user, string message, string to)
        {
            Clients.Caller.SendMessage(user, message);
            Clients.Client(Dic[to].HubId).SendMessage(user, message);
        }

        public void Notify(ChatUser user, string hubId)
        {
            user.HubId = hubId;
            Dic.TryAdd(user.UniqueId, user);
            foreach (var entry in Dic)
            {
                Clients.Caller.Online(entry.Value);
            }

            Clients.Others.Enters(user);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var name = Dic.FirstOrDefault(x => x.Value.HubId == Context.ConnectionId.ToString());
            if (string.IsNullOrEmpty(name.Key))
            {
                return Clients.All.Disconnected("User");
            }

            ChatUser userDisconnecting;
            Dic.TryRemove(name.Key, out userDisconnecting);
            return Clients.All.Disconnected(name.Value);
        }
    }

    public class ChatUser
    {
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public string HubId { get; set; }
        public string AvatarUrl { get; set; }
    }
}