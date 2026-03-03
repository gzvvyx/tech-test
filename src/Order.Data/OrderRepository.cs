using Microsoft.EntityFrameworkCore;
using Order.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;

        public OrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<IEnumerable<OrderSummary>> GetOrdersAsync()
        {
            var orders = await _orderContext.Order
                .Select(x => new OrderSummary
                {
                    Id = new Guid(x.Id),
                    ResellerId = new Guid(x.ResellerId),
                    CustomerId = new Guid(x.CustomerId),
                    StatusId = new Guid(x.StatusId),
                    StatusName = x.Status.Name,
                    ItemCount = x.Items.Count,
                    TotalCost = x.Items.Sum(i => (decimal?)i.Quantity * i.Product.UnitCost) ?? 0,
                    TotalPrice = x.Items.Sum(i => (decimal?)i.Quantity * i.Product.UnitPrice) ?? 0,
                    CreatedDate = x.CreatedDate
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return orders;
        }
        
        public async Task<IEnumerable<OrderSummary>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _orderContext.Order
                .Where(x => Equals(x.Status.Name, status))
                .Select(x => new OrderSummary
                {
                    Id = new Guid(x.Id),
                    ResellerId = new Guid(x.ResellerId),
                    CustomerId = new Guid(x.CustomerId),
                    StatusId = new Guid(x.StatusId),
                    StatusName = x.Status.Name,
                    ItemCount = x.Items.Count,
                    TotalCost = x.Items.Sum(i => (decimal?)i.Quantity * i.Product.UnitCost) ?? 0,
                    TotalPrice = x.Items.Sum(i => (decimal?)i.Quantity * i.Product.UnitPrice) ?? 0,
                    CreatedDate = x.CreatedDate
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            
            return orders;
        }

        public async Task<OrderDetail> GetOrderByIdAsync(Guid orderId)
        {
            var orderIdBytes = orderId.ToByteArray();

            var order = await _orderContext.Order
                .Where(x => _orderContext.Database.IsInMemory() ? x.Id.SequenceEqual(orderIdBytes) : x.Id == orderIdBytes)
                .Select(x => new OrderDetail
                {
                    Id = new Guid(x.Id),
                    ResellerId = new Guid(x.ResellerId),
                    CustomerId = new Guid(x.CustomerId),
                    StatusId = new Guid(x.StatusId),
                    StatusName = x.Status.Name,
                    CreatedDate = x.CreatedDate,
                    TotalCost = x.Items.Sum(i => i.Quantity * i.Product.UnitCost) ?? 0,
                    TotalPrice = x.Items.Sum(i => i.Quantity * i.Product.UnitPrice) ?? 0,
                    Items = x.Items.Select(i => new Model.OrderItem
                    {
                        Id = new Guid(i.Id),
                        OrderId = new Guid(i.OrderId),
                        ServiceId = new Guid(i.ServiceId),
                        ServiceName = i.Service.Name,
                        ProductId = new Guid(i.ProductId),
                        ProductName = i.Product.Name,
                        UnitCost = i.Product.UnitCost,
                        UnitPrice = i.Product.UnitPrice,
                        TotalCost = i.Product.UnitCost * i.Quantity.Value,
                        TotalPrice = i.Product.UnitPrice * i.Quantity.Value,
                        Quantity = i.Quantity.Value
                    })
                }).SingleOrDefaultAsync();
            
            return order;
        }

        public async Task<OrderDetail> UpdateOrderStatusAsync(Guid orderId, string newStatusName)
        {
            var orderIdBytes = orderId.ToByteArray();

            var order = await _orderContext.Order
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id.SequenceEqual(orderIdBytes));

            if (order == null) return null;

            if (order.Status?.Name == newStatusName)
                return await GetOrderByIdAsync(orderId);
            
            var existingStatus = await _orderContext.OrderStatus
                .FirstOrDefaultAsync(x => x.Name == newStatusName);

            if (existingStatus == null)
                throw new InvalidOperationException($"Status {newStatusName} not found");
            
            order.StatusId = existingStatus.Id;
            order.Status =  existingStatus;
            
            await _orderContext.SaveChangesAsync();
            
            return await GetOrderByIdAsync(orderId);
        }

        public async Task<Entities.OrderStatus> GetCreatedStatusAsync()
        {
            var status = await _orderContext.OrderStatus.FirstOrDefaultAsync(x => Equals(x.Name, "Created"));
            return status;
        }

        public async Task CreateOrderAsync(Order.Data.Entities.Order order)
        {
            _orderContext.Order.Add(order);
            
            await _orderContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MonthlyProfit>> CalculateProfitByMonthAsync()
        {
            var orders = await _orderContext.Order
                .Where(x => x.Status.Name == "Completed")
                .Select(x => new
                {
                    x.CreatedDate,
                    TotalCost = x.Items.Sum(i => (decimal?)i.Quantity * i.Product.UnitCost) ?? 0,
                    TotalPrice = x.Items.Sum(i => (decimal?)i.Quantity * i.Product.UnitPrice) ?? 0
                })
                .ToListAsync();

            var profit = orders
                .GroupBy(x => new { x.CreatedDate.Year, x.CreatedDate.Month })
                .Select(g => new MonthlyProfit
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Profit = g.Sum(x => x.TotalPrice) - g.Sum(x => x.TotalCost)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month);

            return profit;
        }
    }
}
