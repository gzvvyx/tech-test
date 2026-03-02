namespace Order.Model.DTO.Extensions;

public static class StatusFilterExtensions
{
    public static string ToStatusName(this StatusFilterType status) => status switch
    {
        StatusFilterType.InProgress => "In Progress",
        StatusFilterType.Created => "Created",
        StatusFilterType.Completed => "Completed",
        StatusFilterType.Shipped => "Shipped",
        StatusFilterType.Failed => "Failed"
    };
}