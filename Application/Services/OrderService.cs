using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Application.DTOs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;

namespace WebApi.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

 public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int pageNumber, int pageSize, OrderDto.OrderStatus? status)
{
    var orders = await _orderRepository.GetAllAsync();

    if (status.HasValue)
    {
        orders = orders.Where(o => o.Status == (Order.OrderStatus)status.Value);
    }

    var pagedOrders = orders
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    return pagedOrders.Select(order => new OrderDto
    {
        Id = order.Id,
        OrderName = order.OrderName,
        ClientName = order.ClientName,
        OrderItems = order.OrderItems?.Select(item => new OrderItemDto
        {
            Id = item.Id,
            ItemId = item.ItemId,
            Quantity = item.Quantity,
            Price = item.Price
        }).ToList() ?? new List<OrderItemDto>(),
        Status = (OrderDto.OrderStatus)order.Status
    });
}
        public async Task<OrderDto> GetOrderByIdAsync(int id)
{
    var order = await _orderRepository.GetByIdAsync(id);
    if (order == null)
    {
        return new OrderDto(); // Retorna um objeto vazio ao invés de null
    }

    return new OrderDto
    {
        Id = order.Id,
        OrderName = order.OrderName,
        ClientName = order.ClientName,
        OrderItems = order.OrderItems?.Select(item => new OrderItemDto
        {
            Id = item.Id,
            ItemId = item.ItemId,
            Quantity = item.Quantity,
            Price = item.Price
        }).ToList() ?? new List<OrderItemDto>(),
        Status = (OrderDto.OrderStatus)order.Status
    };
}

public async Task AddOrderAsync(OrderDto orderDto)
{
    if (orderDto == null)
    {
        throw new ArgumentNullException(nameof(orderDto), "Order DTO cannot be null.");
    }

    var order = new Order
    {
        OrderName = orderDto.OrderName,
        ClientName = orderDto.ClientName,
        OrderItems = orderDto.OrderItems?.Select(item => new OrderItem
        {
            ItemId = item.ItemId,
            Quantity = item.Quantity,
            Price = item.Price
        }).ToList() ?? new List<OrderItem>(), // Garante que não seja nulo
    };

    // Define o status usando o método AtualizarStatus
    order.AtualizarStatus(Order.OrderStatus.Aberto); // Chama o método para definir o status

    await _orderRepository.AddAsync(order);
}


        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderDto.Id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {orderDto.Id} not found.");

            // Atualizando propriedades
            order.OrderName = orderDto.OrderName;
            order.ClientName = orderDto.ClientName;
            order.OrderItems = orderDto.OrderItems.Select(item => new OrderItem
            {
                Id = item.Id,
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            // Atualizando o status utilizando o método definido na classe Order
            order.AtualizarStatus((Order.OrderStatus)orderDto.Status);

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with id {id} not found.");

            await _orderRepository.DeleteAsync(order);
        }
    }
}
