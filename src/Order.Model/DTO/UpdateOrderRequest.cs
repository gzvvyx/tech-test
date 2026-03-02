using System;

namespace Order.Model.DTO;

public class UpdateOrderRequest
{
    public Guid Id { get; set; }
    public StatusFilter Status { get; set; }
}