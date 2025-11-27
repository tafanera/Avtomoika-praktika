using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Application;
using WebApp.Controllers;
using WebApp.Application.Interfaces;
using WebApp.models;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _mockService;
    private readonly Mock<ILogger<OrderController>> _mockLogger;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mockService = new Mock<IOrderService>();
        _mockLogger = new Mock<ILogger<OrderController>>();
        _controller = new OrderController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Order_Test1()
    {
        var order = new Order
        {
            Id = 1,
            UserId = 10,
            CarId = 5,
            Services = new List<Service> {
                new Service { Id = 1, Name = "Мойка", Price = 500 }
            },
            OrderDate = DateTime.Today,
            TotalPrice = 500,
            Status = "Ожидание"
        };

        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(order);

        var result = await _controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var returned = Assert.IsType<Order>(ok.Value);
        Assert.Equal(1, returned.Id);
        Assert.Equal(10, returned.UserId);
        Assert.Equal(5, returned.CarId);
        Assert.Single(returned.Services);
        Assert.Equal(500, returned.TotalPrice);
    }

    [Fact]
    public async Task Order_Test2()
    {
        var dto = new OrderDto
        {
            BuyerId = 10,
            CarId = 2,
            ServiceIds = new List<int> { 1 },
        };

        var created = new Order
        {
            Id = 5,
            UserId = 10,
            CarId = 2,
            TotalPrice = 1000,
            Status = "Ожидание"
        };

        _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        var result = await _controller.Create(dto);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

        var returned = Assert.IsType<Order>(createdResult.Value);
        Assert.Equal(5, returned.Id);
        Assert.Equal(1000, returned.TotalPrice);
    }

    [Fact]
    public async Task Order_Test3()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Order>
        {
            new Order { Id = 1, UserId = 2 },
            new Order { Id = 2, UserId = 3 },
        });

        var result = await _controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var list = Assert.IsType<List<Order>>(ok.Value);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task Order_Test4()
    {
        var order = new Order { Id = 3, UserId = 2, CarId = 1 };

        _mockService.Setup(s => s.UpdateAsync(order)).ReturnsAsync(true);

        var result = await _controller.Update(3, order);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Order_Test5()
    {
        _mockService.Setup(s => s.GetByIdAsync(77)).ReturnsAsync((Order?)null);

        var result = await _controller.GetById(77);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Order_Test6()
    {
        var order = new Order { Id = 99 };

        var result = await _controller.Update(1, order);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Order_Test7()
    {
        _mockService.Setup(s => s.DeleteAsync(33)).ReturnsAsync(false);

        var result = await _controller.Delete(33);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
