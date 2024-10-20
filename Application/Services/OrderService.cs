using WebApi.Application.DTOs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Infraestructure.Repositories;

namespace WebApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderName = order.OrderName,
                ClientName = order.ClientName,
                Price = order.Price,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList() ?? new List<OrderItemDto>()
            });
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            return new OrderDto
            {
                Id = order.Id,
                OrderName = order.OrderName,
                ClientName = order.ClientName,
                Price = order.Price,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ItemId = oi.ItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList() ?? new List<OrderItemDto>()
            };
        }

        public async Task AddOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderName = orderDto.OrderName,
                ClientName = orderDto.ClientName,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderDto.OrderItems.Select(oi => new OrderItem
                {
                    ItemId = oi.ItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            order.OrderName = orderDto.OrderName;
            order.ClientName = orderDto.ClientName;
            order.UpdatedAt = DateTime.UtcNow;
            order.OrderItems = orderDto.OrderItems.Select(oi => new OrderItem
            {
                Id = oi.Id,
                ItemId = oi.ItemId,
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList();

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Order not found");

            await _orderRepository.DeleteAsync(order);
        }
    }
}