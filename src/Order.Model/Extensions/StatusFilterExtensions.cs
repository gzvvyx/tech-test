namespace Order.Model.Extensions;

public static class StatusExtensions
{
    public static string ToStatusName(this OrderStatus status) => status switch
    {
        OrderStatus.InProgress => "In Progress",
        OrderStatus.Created => "Created",
        OrderStatus.Completed => "Completed",
        OrderStatus.Failed => "Failed"
    };
}