namespace Order.Model.DTO;

public class StatusFilter
{
    public StatusFilterType Status { get; set; }
}

public enum StatusFilterType
{
    Completed,
    Created,
    InProgress,
    Shipped,
    Failed
}