using Order.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order.Model.DTO;

namespace Order.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderSummary>> GetOrdersAsync();
        Task<IEnumerable<OrderSummary>> GetOrdersByStatusAsync(StatusFilter status);
        Task<OrderDetail> GetOrderByIdAsync(Guid orderId);
        Task<OrderDetail> UpdateOrderStatusAsync(UpdateOrderRequest request);
        Task<OrderDetail> CreateOrderAsync(CreateOrderRequest request);
        Task<IEnumerable<MonthlyProfit>> CalculateProfitByMonthAsync();
    }
}
