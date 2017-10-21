using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Echelon.Infrastructure.Messaging
{
  public class ChatHub : Hub
  {
    private static readonly ConcurrentDictionary<string, string> Dic = new ConcurrentDictionary<string, string>();

    public void Send(string name, string message)
    {
      // Call the broadcastMessage method to update clients.
      Clients.All.broadcastMessage(name, message);
    }

    public void SendToSpecific(string name, string message, string to)
    {
      // Call the broadcastMessage method to update clients.
      Clients.Caller.broadcastMessage(name, message);
      Clients.Client(Dic[to]).broadcastMessage(name, message);
    }

    public void Notify(string name, string id)
    {
      if (Dic.ContainsKey(name))
      {
        Clients.Caller.differentName();
      }
      else
      {
        Dic.TryAdd(name, id);

        foreach (KeyValuePair<String, String> entry in Dic)
        {
          Clients.Caller.online(entry.Key);
        }

        Clients.Others.enters(name);
      }
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      var name = Dic.FirstOrDefault(x => x.Value == Context.ConnectionId.ToString());
      string s;
      Dic.TryRemove(name.Key, out s);
      return Clients.All.disconnected(name.Key);
    }
  }
}