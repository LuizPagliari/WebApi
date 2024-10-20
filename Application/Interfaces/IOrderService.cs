using WebApi.Application.DTOs;

namespace WebApi.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int pageNumber, int pageSize, OrderDto.OrderStatus? status);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task AddOrderAsync(OrderDto orderDto);
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(int id);
    }
}