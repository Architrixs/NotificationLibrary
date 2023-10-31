using Microsoft.AspNetCore.SignalR;

namespace i2vNotificationLibrary;

// This class acts as a Mediator for Sending Notification to client using SignalR
// this will have different methods for sending notification to different clients based on ehich clients to send to
// this will also have methods for sending notification to all clients
public class NotificationSender
{
    private IHubContext<Hub> _hubContext; // Store the hub context
    public void SetHubContext(IHubContext<Hub> hubContext)
    {
        // Set the hub context for the specified hub
        _hubContext = hubContext;
    }
    
    public async Task SendNotificationToUser(string methodName, string userId, Notification notification)
    {
        // check for HubContext
        if (_hubContext == null) throw new InvalidOperationException("Hub context not set");
        await _hubContext.Clients.User(userId).SendAsync(methodName, notification);
    }

    public async Task SendNotificationToGroup(string methodName, string groupName, Notification notification)
    {
        if (_hubContext == null) throw new InvalidOperationException("Hub context not set");
        await _hubContext.Clients.Group(groupName).SendAsync(methodName, notification);
    }

    public async Task SendNotificationToAll(string methodName, Notification notification)
    {
        if (_hubContext == null) throw new InvalidOperationException("Hub context not set, Set it using method NotificationLibrary.SetHubContext(IHubContext<Hub> hubContext)");
        await _hubContext.Clients.All.SendAsync(methodName, notification);
    }
    
    public async Task SendNotificationToAllExcept(string methodName, string userId, Notification notification)
    {
        if (_hubContext == null) throw new InvalidOperationException("Hub context not set");
        await _hubContext.Clients.AllExcept(userId).SendAsync(methodName, notification);
    }
    
    public async Task SendNotificationToConnections(string methodName, List<string> connectionIds, Notification notification)
    {
        if (_hubContext == null) throw new InvalidOperationException("Hub context not set");
        await _hubContext.Clients.Clients(connectionIds).SendAsync(methodName, notification);
    }
}