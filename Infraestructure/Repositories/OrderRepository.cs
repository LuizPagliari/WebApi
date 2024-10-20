using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infraestructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

       public async Task<Order> GetByIdAsync(int id)
{
    var order = await _context.Orders.Include(o => o.OrderItems)
                                     .FirstOrDefaultAsync(o => o.Id == id);
    if (order == null)
    {
        throw new KeyNotFoundException($"Order with ID {id} not found.");
    }
    return order;
}

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}