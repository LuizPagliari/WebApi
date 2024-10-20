using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infraestructure.Repositories
{
    public class ItemRepository(ApplicationDbContext context) : IItemRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetByIdAsync(int id)
{
    var item = await _context.Items.FindAsync(id);
    if (item == null)
    {
        throw new KeyNotFoundException($"Item with ID {id} not found.");
    }
    return item;
}

        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}