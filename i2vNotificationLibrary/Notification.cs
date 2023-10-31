namespace i2vNotificationLibrary;

public class Notification
{
    public string OperationType { get; set; }
    public string EntityName { get; set; }
    public string Message { get; set; }
    public object Resource { get; set; }
}