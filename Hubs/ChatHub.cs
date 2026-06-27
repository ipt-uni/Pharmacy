using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace pharmacy.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(string message)
    {
        var username = Context.User?.Identity?.Name ?? "Unknown";
        Console.WriteLine($"Sending message to {username}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}
