using WebApi.Domain.Entities;

namespace WebApi.Domain.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(int orderId, int itemId);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(int orderId, int itemId);
    }
}
