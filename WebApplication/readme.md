## This library requires a SignalR hub service to use `IHubContext`. Here's how to set it up:
### Create a NotificationHub Extending Hub
```C#
public class NotificationHub : Hub
{
    // Hub implementation
}
```
### Register Library Services
Register the library services, such as NotificationLibrary and NotificationSender, as singleton services in your application's dependency injection container:
```C#
services.AddSingleton<NotificationLibrary>();
services.AddSingleton<NotificationSender>();
```
### Get Your Library Instance and Set the HubContext
In your NotificationHub constructor, set up the IHubContext and initialize your library:

```C#
public NotificationHub(IHubContext<NotificationHub> hub, NotificationLibrary notificationLibrary)
{
    notificationHub = hub;
    notificationLibrary.SetHubContext(notificationHub);
    
    // Register your entities
    notificationLibrary.RegisterEntity<VideoSource>("VideoSource");
    notificationLibrary.RegisterEntity<AnalyticServer>("AnalyticServer");
    notificationLibrary.RegisterEntity<User>("User");
}
```
### Basic Usage
You can inject the NotificationLibrary in a controller and call its Method like so:
```C#
_notificationLibrary.SendNotificationToAll("crudNotification", OperationType.Update, videoSource, message);
```
