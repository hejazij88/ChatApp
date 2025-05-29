using ChatApp.Data;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using ChatApp.Models;

namespace ChatApp.Hub;

public class ChatHub:Microsoft.AspNetCore.SignalR.Hub
{
    private readonly ChatDbContext _db;

    public ChatHub(ChatDbContext db)
    {
        _db = db;
    }
    private static ConcurrentDictionary<string, string> _users = new();
    private static ConcurrentBag<string> _groups = new();


    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public async Task CreateGroup(string groupName)
    {
        if (!_groups.Contains(groupName))
        {
            _groups.Add(groupName);
            await Clients.All.SendAsync("GroupListUpdated", _groups.ToList());
        }
    }
    public async Task LoadGroupMessages(string group)
    {
        var messages = _db.Messages
            .Where(m => m.Group == group)
            .OrderBy(m => m.Timestamp)
            .Select(m => new
            {
                m.Sender,
                m.Text,
                Timestamp = m.Timestamp.ToString("HH:mm")
            })
            .ToList();

        await Clients.Caller.SendAsync("LoadGroupHistory", messages);
    }
    public async Task RequestGroupList()
    {
        await Clients.Caller.SendAsync("GroupListUpdated", _groups.ToList());
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var user = _users.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (user != null)
            _users.TryRemove(user, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task RegisterUser(string username)
    {
        _users[username] = Context.ConnectionId;
        await Clients.All.SendAsync("UserListUpdated", _users.Keys.ToList());
    }

    public async Task SendToGroup(string group, string user, string message)
    {
        var msg = new ChatMessage
        {
            Sender = user,
            Group = group,
            Text = message
        };
        _db.Messages.Add(msg);
        await _db.SaveChangesAsync();

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

    public async Task SendPrivate(string toUser, string fromUser, string message)
    {
        if (_users.TryGetValue(toUser, out var connId))
        {
            var msg = new ChatMessage
            {
                Sender = fromUser,
                Receiver = toUser,
                Text = message
            };
            _db.Messages.Add(msg);
            await _db.SaveChangesAsync();

            await Clients.Client(connId).SendAsync("ReceivePrivate", fromUser, message);
        }
    }
}