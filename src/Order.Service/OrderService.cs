using Order.Data;
using Order.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.Model.DTO;
using Order.Model.Extensions;

namespace Order.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderSummary>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return orders;
        }

        public async Task<OrderDetail> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            return order;
        }

        public async Task<IEnumerable<OrderSummary>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status.ToStatusName());
            return orders;
        }

        public async Task<OrderDetail> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.UpdateOrderStatusAsync(orderId, status.ToStatusName());
            return order;
        }

        public async Task<OrderDetail> CreateOrderAsync(CreateOrderRequest request)
        {
            var createdStatus = await _orderRepository.GetCreatedStatusAsync();
            
            var orderId = Guid.NewGuid();

            var order = new Data.Entities.Order
            {
                Id = orderId.ToByteArray(),
                ResellerId = request.ResellerId.ToByteArray(),
                CustomerId = request.CustomerId.ToByteArray(),
                StatusId = createdStatus.Id,
                CreatedDate = DateTime.UtcNow,
                Status = createdStatus,
                Items = request.OrderItems.Select(i => new Data.Entities.OrderItem
                {
                    Id = Guid.NewGuid().ToByteArray(),
                    ProductId = i.ProductId.ToByteArray(),
                    ServiceId =  i.ServiceId.ToByteArray(),
                    OrderId = orderId.ToByteArray(),
                    Quantity = i.Quantity
                }).ToHashSet()
            };
            
            await _orderRepository.CreateOrderAsync(order);

            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<MonthlyProfit>> CalculateProfitByMonthAsync()
        {
            return await _orderRepository.CalculateProfitByMonthAsync();
        }
    }
}
