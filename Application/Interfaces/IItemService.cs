using WebApi.Application.DTOs;

namespace WebApi.Application.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto> GetItemByIdAsync(int id);
        Task AddItemAsync(ItemDto itemDto);
        Task UpdateItemAsync(ItemDto itemDto);
        Task DeleteItemAsync(int id);
    }
}