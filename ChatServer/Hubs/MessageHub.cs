using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    /// <summary>
    /// The Message Hub 
    /// </summary>
    [Authorize]
    public class MessageHub : Hub
    {
        public async Task ConnectToGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveFromGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
