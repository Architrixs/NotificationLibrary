using Microsoft.AspNetCore.SignalR;

namespace i2vNotificationLibrary;

public class NotificationLibrary
{
    private readonly NotificationSender _notificationSender;
    private readonly Dictionary<Type, string> _entityTypeMapping;
    public static HashSet<string> ConnectedIds = new HashSet<string>();
    public NotificationLibrary(NotificationSender notificationSender)
    {

        _notificationSender = notificationSender;
        _entityTypeMapping = new Dictionary<Type, string>();
    }

    /// <summary>
    ///  Add Hub from main application
    /// </summary>
    /// <param name="hubContext">Hub context from main application</param>
    public void SetHubContext(IHubContext<Hub> hubContext)
    {
        _notificationSender.SetHubContext(hubContext);
    }

    /// <summary>
    ///  Register entity type with entity name
    /// </summary>
    /// <param name="entityName"></param>
    /// <typeparam name="T"></typeparam>
     
    public void RegisterEntity<T>(string entityName)
    {
        if (string.IsNullOrWhiteSpace(entityName)) throw new ArgumentException(nameof(entityName));
        _entityTypeMapping.Add(typeof(T), entityName);
    }

    /// <summary>
    ///  Send notification to a specific user
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="userId"></param>
    /// <param name="operationType"></param>
    /// <param name="resource"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task SendNotificationToUser<T>(string methodName, string userId, string operationType, T resource, string message = "")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException(nameof(methodName));
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrWhiteSpace(operationType)) throw new ArgumentException(nameof(operationType));
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (!_entityTypeMapping.ContainsKey(typeof(T))) throw new InvalidOperationException($"No entity type mapping found for {typeof(T)}");

            var notification = new Notification
            {
                OperationType = operationType,
                EntityName = _entityTypeMapping[typeof(T)],
                Resource = resource,
                Message = message
            };

            await _notificationSender.SendNotificationToUser(methodName, userId, notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to user: {userId} , error: {ex.Message}");
        }
    }

    public async Task SendNotificationToGroup<T>(string methodName, string groupName, string operationType, T resource, string message = "")
    {
        try
        {

            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentException(nameof(methodName));
            if (string.IsNullOrWhiteSpace(groupName)) throw new ArgumentException(nameof(groupName));
            if (string.IsNullOrWhiteSpace(operationType)) throw new ArgumentException(nameof(operationType));
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (!_entityTypeMapping.ContainsKey(typeof(T))) throw new InvalidOperationException($"No entity type mapping found for {typeof(T)}");

            var notification = new Notification
            {
                OperationType = operationType,
                EntityName = _entityTypeMapping[typeof(T)],
                Resource = resource,
                Message = message
            };

            await _notificationSender.SendNotificationToGroup(methodName, groupName, notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to group: {groupName} , error: {ex.Message}");
        }
    }

    public async Task SendNotificationToAll<T>(string methodName, string operationType, T resource, string message = "")
    {
        try
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentException("Method name is required", nameof(methodName));
            if (string.IsNullOrEmpty(operationType)) throw new ArgumentException("Operation type is required", nameof(operationType));
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (!_entityTypeMapping.ContainsKey(typeof(T))) throw new InvalidOperationException($"No entity type mapping found for {typeof(T)}");

            var notification = new Notification
            {
                OperationType = operationType,
                EntityName = _entityTypeMapping[typeof(T)],
                Resource = resource,
                Message = message
            };
            await _notificationSender.SendNotificationToAll(methodName, notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to all , error: {ex.Message}");
        }

    }

    public async Task SendNotificationToAllExcept<T>(string methodName, string userId, string operationType, T resource, string message = "")
    {
        try
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            if (string.IsNullOrEmpty(operationType)) throw new ArgumentNullException(nameof(operationType));
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (!_entityTypeMapping.ContainsKey(typeof(T))) throw new ArgumentException($"There is no mapping for type {typeof(T)}");

            var notification = new Notification
            {
                OperationType = operationType,
                EntityName = _entityTypeMapping[typeof(T)],
                Resource = resource,
                Message = message
            };
            await _notificationSender.SendNotificationToAllExcept(methodName, userId, notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to all except user: {userId} , error: {ex.Message}");
        }
    }

    // to multiple connections
    public async Task SendNotificationToConnections<T>(string methodName, List<string> connectionIds, string operationType, T resource, string message = "")
    {
        try
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (connectionIds == null || !connectionIds.Any()) throw new ArgumentNullException(nameof(connectionIds));
            if (string.IsNullOrEmpty(operationType)) throw new ArgumentNullException(nameof(operationType));
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (!_entityTypeMapping.ContainsKey(typeof(T))) throw new ArgumentException($"There is no mapping for type {typeof(T)}");

            var notification = new Notification
            {
                OperationType = operationType,
                EntityName = _entityTypeMapping[typeof(T)],
                Resource = resource,
                Message = message
            };
            await _notificationSender.SendNotificationToConnections(methodName, connectionIds, notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to connections , error: {ex.Message}");
        }
    }

}

public class OperationType
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
}
