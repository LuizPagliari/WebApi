using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Application.DTOs;
using WebApi.Application.Interfaces;
using WebApi.Application.Services;
using WebApi.Domain.Entities;
using WebApi.Infraestructure.Repositories;
using Xunit;

namespace WebApi.Tests.UnitTests
{
    public class ItemServiceTest
    {
        private readonly Mock<ItemRepository> _itemRepositoryMock;
        private readonly ItemService _itemService;

        public ItemServiceTest()
        {
            _itemRepositoryMock = new Mock<ItemRepository>();
            _itemService = new ItemService(_itemRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnAllItems()
        {
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Item1", Quantity = 10, Price = 100, Description = "Description1", Category = "Category1" },
                new Item { Id = 2, Name = "Item2", Quantity = 20, Price = 200, Description = "Description2", Category = "Category2" }
            };
            _itemRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(items);

            var result = await _itemService.GetAllItemsAsync();

            Assert.Equal(2, result.Count());
            Assert.Equal("Item1", result.First().Name);
        }

        [Fact]
        public async Task GetItemByIdAsync_ShouldReturnItem_WhenItemExists()
        {
            var item = new Item { Id = 1, Name = "Item1", Quantity = 10, Price = 100, Description = "Description1", Category = "Category1" };
            _itemRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);

            var result = await _itemService.GetItemByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Item1", result.Name);
        }

     [Fact]
public async Task GetItemByIdAsync_ShouldThrowKeyNotFoundException_WhenItemDoesNotExist()
{
    _itemRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Item)null!);

    await Assert.ThrowsAsync<KeyNotFoundException>(() => _itemService.GetItemByIdAsync(1));
}


        [Fact]
        public async Task AddItemAsync_ShouldAddItem()
        {
            var itemDto = new ItemDto { Name = "NewItem", Quantity = 5, Price = 50, Description = "NewDescription", Category = "NewCategory" };

            await _itemService.AddItemAsync(itemDto);

            _itemRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Item>()), Times.Once);
        }

        [Fact]
        public async Task UpdateItemAsync_ShouldUpdateItem_WhenItemExists()
        {
            var item = new Item { Id = 1, Name = "Item1", Quantity = 10, Price = 100, Description = "Description1", Category = "Category1" };
            var itemDto = new ItemDto { Id = 1, Name = "UpdatedItem", Quantity = 15, Price = 150, Description = "UpdatedDescription", Category = "UpdatedCategory" };
            _itemRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);

            await _itemService.UpdateItemAsync(itemDto);

            _itemRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Item>()), Times.Once);
        }

      [Fact]
public async Task UpdateItemAsync_ShouldThrowKeyNotFoundException_WhenItemDoesNotExist()
{
    var itemDto = new ItemDto 
    { 
        Id = 1, 
        Name = "UpdatedItem", 
        Quantity = 15, 
        Price = 150, 
        Description = "UpdatedDescription", 
        Category = "UpdatedCategory" 
    };

    _itemRepositoryMock.Setup(repo => repo.GetByIdAsync(itemDto.Id)).ReturnsAsync((Item)null!);

    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _itemService.UpdateItemAsync(itemDto));

    Assert.Equal($"Item with id {itemDto.Id} not found.", exception.Message);
}


        [Fact]
        public async Task DeleteItemAsync_ShouldDeleteItem_WhenItemExists()
        {
            var item = new Item { Id = 1, Name = "Item1", Quantity = 10, Price = 100, Description = "Description1", Category = "Category1" };
            _itemRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);

            await _itemService.DeleteItemAsync(1);

            _itemRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Item>()), Times.Once);
        }

[Fact]
public async Task DeleteItemAsync_ShouldThrowKeyNotFoundException_WhenItemDoesNotExist()
{
    _itemRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Item)null!);

    var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _itemService.DeleteItemAsync(1));

    Assert.Equal("Item with id 1 not found.", exception.Message);
}
    }
}
