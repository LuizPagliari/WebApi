using WebApi.Domain.Entities;

namespace WebApi.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(int id);
        Task<IEnumerable<Item>> GetAllAsync();
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(int id);
    }
}
