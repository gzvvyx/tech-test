using System;
using System.Collections.Generic;

namespace Order.Model.DTO;

public class CreateOrderRequest
{
    public Guid ResellerId { get; set; }
    public Guid CustomerId { get; set; }
    public IEnumerable<CreateOrderItemRequest> OrderItems { get; set; }
}