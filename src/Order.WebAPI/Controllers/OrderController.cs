using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Service;
using System;
using System.Threading.Tasks;
using Order.Model.DTO;
using Order.WebAPI.Validators;

namespace Order.WebAPI.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order is null) return NotFound();

            return Ok(order);
        }
        
        [HttpGet("{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrdersByStatus(StatusFilter status)
        {
            var validator = new StatusValidator();
            var validationResult = await validator.ValidateAsync(status);
            if (!validationResult.IsValid) return BadRequest();
            
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPatch("{orderId}/{newStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderRequest request)
        {
            var validator = new UpdateOrderRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return BadRequest();
            
            var order = await _orderService.UpdateOrderStatusAsync(request);
            if (order is null) return NotFound();
            
            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var validator = new CreateOrderRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid) return BadRequest();
            
            var response = await _orderService.CreateOrderAsync(request);
            return Ok(response);
        }

        [HttpGet("profit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfit()
        {
            var profit = await _orderService.CalculateProfitByMonthAsync();
            return Ok(profit);
        }
    }
}
