using WebApi.Application.DTOs;
using WebApi.Application.Interfaces;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository; 

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Price = item.Price,
                Description = item.Description,
                Category = item.Category
            });
        }

        public async Task<ItemDto> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Item not found");

            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Price = item.Price,
                Description = item.Description,
                Category = item.Category
            };
        }

        public async Task AddItemAsync(ItemDto itemDto)
        {
            var item = new Item
            {
                Name = itemDto.Name,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
                Description = itemDto.Description,
                Category = itemDto.Category,
                CreatedAt = DateTime.UtcNow
            };

            await _itemRepository.AddAsync(item);
        }

        public async Task UpdateItemAsync(ItemDto itemDto)
        {
            var item = await _itemRepository.GetByIdAsync(itemDto.Id);
            if (item == null)
                throw new KeyNotFoundException("Item not found");

            item.Name = itemDto.Name;
            item.Quantity = itemDto.Quantity;
            item.Price = itemDto.Price;
            item.Description = itemDto.Description;
            item.Category = itemDto.Category;
            item.UpdatedAt = DateTime.UtcNow;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException("Item not found");

            await _itemRepository.DeleteAsync(item);
        }
    }
}