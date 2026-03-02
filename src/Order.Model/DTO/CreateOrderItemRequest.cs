using System;

namespace Order.Model.DTO;

public class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }
    public Guid ServiceId { get; set; }
    public int Quantity { get; set; }
}