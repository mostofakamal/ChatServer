using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    /// <summary>
    /// 
    /// </summary>
   // [Authorize]
    public class MessageHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }

        public async Task SendMessageToGroup(Message msg, string groupName)
        {
            await Clients.Group(groupName).SendAsync("MessageReceived", msg);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
