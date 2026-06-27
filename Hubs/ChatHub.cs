using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace pharmacy.Hubs;

/// <summary>
/// SignalR hub for real-time staff-to-staff chat.
/// Only authenticated users can connect.
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    /// <summary>
    /// Broadcasts a message from the connected user to all connected clients.
    /// The client-side "ReceiveMessage" handler receives (username, message).
    /// </summary>
    public async Task SendMessage(string message)
    {
        var username = Context.User?.Identity?.Name ?? "Unknown";
        Console.WriteLine($"Sending message to {username}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}
