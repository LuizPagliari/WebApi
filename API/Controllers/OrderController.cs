using Microsoft.AspNetCore.Mvc;
using WebApi.Application.DTOs;
using WebApi.Application.Interfaces;

namespace WebApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            await _orderService.AddOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.Id }, orderDto);
        }

        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AddItemToOrder(int orderId, [FromBody] OrderItemDto orderItemDto)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            order.OrderItems.Add(orderItemDto);
            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromOrder(int orderId, int itemId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            var item = order.OrderItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return NotFound();

            order.OrderItems.Remove(item);
            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpPost("{orderId}/close")]
        public async Task<IActionResult> CloseOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            if (!order.OrderItems.Any())
                return BadRequest("Order must contain at least one item to be closed.");

           
            order.Status = OrderDto.OrderStatus.Fechado;
            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}