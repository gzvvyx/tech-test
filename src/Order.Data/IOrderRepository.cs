using Order.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Data.Entities;

namespace Order.Data
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderSummary>> GetOrdersAsync();
        Task<IEnumerable<OrderSummary>> GetOrdersByStatusAsync(string status);
        Task<OrderStatus> GetCreatedStatusAsync();
        Task<OrderDetail> GetOrderByIdAsync(Guid orderId);
        Task<OrderDetail> UpdateOrderStatusAsync(Guid orderId, string newStatusName);
        Task CreateOrderAsync(Order.Data.Entities.Order order);
        Task<IEnumerable<MonthlyProfit>> CalculateProfitByMonthAsync();
    }
}
