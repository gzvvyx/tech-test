using System;
using System.Collections.Generic;

namespace Order.Model.DTO;

public class CreateOrderDto
{
    public Guid ResellerId { get; set; }
    public Guid CustomerId { get; set; }
    public IEnumerable<CreateOrderItemDto> OrderItems { get; set; }
}