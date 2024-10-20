using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infraestructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> GetByIdAsync(int id)
{
    var orderItem = await _context.OrderItems.FindAsync(id);
    if (orderItem == null)
    {
        throw new KeyNotFoundException($"OrderItem with ID {id} not found.");
    }
    return orderItem;
}


        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}