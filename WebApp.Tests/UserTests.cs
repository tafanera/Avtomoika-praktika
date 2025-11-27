using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Controllers;
using WebApp.Application.Interfaces;
using WebApp.models;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockService;
    private readonly Mock<ILogger<UsersController>> _mockLogger;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mockService = new Mock<IUserService>();
        _mockLogger = new Mock<ILogger<UsersController>>();
        _controller = new UsersController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Users_Test1()
    {
        var user = new User
        {
            Id = 1,
            Name = "Иван Романов",
            Number = "+79995553322",
            Orders = new List<Order>
            {
                new Order { Id = 10, TotalPrice = 3000 }
            }
        };

        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

        var result = await _controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var returned = Assert.IsType<User>(ok.Value);
        Assert.Equal("Иван Романов", returned.Name);
        Assert.Equal("+79995553322", returned.Number);
        Assert.Single(returned.Orders);
    }

    [Fact]
    public async Task Users_Test2()
    {
        var newUser = new User
        {
            Id = 5,
            Name = "Глеб Орлов",
            Number = "89997774455"
        };

        _mockService.Setup(s => s.CreateAsync(newUser)).ReturnsAsync(newUser);

        var result = await _controller.Create(newUser);
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);

        var returned = Assert.IsType<User>(created.Value);
        Assert.Equal("Глеб Орлов", returned.Name);
    }

    [Fact]
    public async Task Users_Test3()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<User>
        {
            new User { Id = 1, Name = "A" },
            new User { Id = 2, Name = "Б" }
        });

        var result = await _controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var list = Assert.IsType<List<User>>(ok.Value);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task Users_Test4()
    {
        _mockService.Setup(s => s.GetByIdAsync(55)).ReturnsAsync((User?)null);

        var result = await _controller.GetById(55);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Users_Test5()
    {
        var user = new User { Id = 3 };

        var result = await _controller.Update(1, user);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Users_Test6()
    {
        _mockService.Setup(s => s.DeleteAsync(10)).ReturnsAsync(false);

        var result = await _controller.Delete(10);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
