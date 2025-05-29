using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hub;

public class ChatHub:Microsoft.AspNetCore.SignalR.Hub
{
    public async Task SendToGroup(string group, string user, string message)
    {
        await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinGroup(string group)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    public async Task LeaveGroup(string group)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }

    public async Task SendPrivate(string receiverConnectionId, string sender, string message)
    {
        await Clients.Client(receiverConnectionId).SendAsync("ReceivePrivate", sender, message);
    }
}