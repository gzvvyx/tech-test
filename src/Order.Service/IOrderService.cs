using Order.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Data.Entities;

namespace Order.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderSummary>> GetOrdersAsync();
        Task<IEnumerable<OrderSummary>> GetOrdersFailedAsync();
        Task<OrderDetail> GetOrderByIdAsync(Guid orderId);
        Task<OrderDetail> UpdateOrderStatusAsync(Guid orderId, string newStatus);
    }
}
