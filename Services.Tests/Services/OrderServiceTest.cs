using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using WebApi.Application.DTOs;
using WebApi.Application.Interfaces;
using WebApi.Application.Services;
using WebApi.Domain.Entities;
using WebApi.Infraestructure.Repositories;
using Xunit;

namespace WebApi.Tests.UnitTests
{
    public class OrderServiceTest
    {
        private readonly Mock<OrderRepository> _orderRepositoryMock;
        private readonly OrderService _orderService;

        public OrderServiceTest()
        {
            _orderRepositoryMock = new Mock<OrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
        }

[Fact]
public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
{
    
    var orders = new List<Order>
    {
        new Order { Id = 1, OrderName = "Order1", ClientName = "Client1" },
        new Order { Id = 2, OrderName = "Order2", ClientName = "Client2" }
    };

    _orderRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

    int pageNumber = 1;
    int pageSize = 10;
    OrderDto.OrderStatus? status = null;  

    
    var result = await _orderService.GetAllOrdersAsync(pageNumber, pageSize, status);

    // Assert
    Assert.Equal(2, result.Count());
    Assert.Equal("Order1", result.First().OrderName);
}


        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, OrderName = "Order1", ClientName = "Client1" };
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Order1", result.OrderName);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldThrowKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Order)null!); 

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.GetOrderByIdAsync(1));
        }

        [Fact]
        public async Task AddOrderAsync_ShouldAddOrder()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                OrderName = "Order1",
                ClientName = "Client1",
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { ItemId = 1, Quantity = 2, Price = 50 }
                }
            };

            // Act
            await _orderService.AddOrderAsync(orderDto);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, OrderName = "Order1", ClientName = "Client1" };
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);

            var orderDto = new OrderDto
            {
                Id = 1,
                OrderName = "UpdatedOrder",
                ClientName = "UpdatedClient",
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { Id = 1, ItemId = 1, Quantity = 2, Price = 50 }
                }
            };

            // Act
            await _orderService.UpdateOrderAsync(orderDto);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldThrowKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Order)null!);

            var orderDto = new OrderDto
            {
                Id = 1,
                OrderName = "UpdatedOrder",
                ClientName = "UpdatedClient",
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { Id = 1, ItemId = 1, Quantity = 2, Price = 50 }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.UpdateOrderAsync(orderDto));
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder_WhenOrderExists()
        {
            // Arrange
            var order = new Order { Id = 1, OrderName = "Order1", ClientName = "Client1" };
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(order);

            // Act
            await _orderService.DeleteOrderAsync(1);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldThrowKeyNotFoundException_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Order)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _orderService.DeleteOrderAsync(1));
        }
    }
}
