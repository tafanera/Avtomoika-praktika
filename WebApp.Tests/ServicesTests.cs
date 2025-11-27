using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApp.Controllers;
using WebApp.Application.Interfaces;
using WebApp.models;

public class ServiceControllerTests
{
    private readonly Mock<IServiceService> _mockService;
    private readonly Mock<ILogger<ServiceController>> _mockLogger;
    private readonly ServiceController _controller;

    public ServiceControllerTests()
    {
        _mockService = new Mock<IServiceService>();
        _mockLogger = new Mock<ILogger<ServiceController>>();
        _controller = new ServiceController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetById_Returns_Service_With_Price()
    {
        var service = new Service
        {
            Id = 1,
            Name = "Полировка",
            Opisanie = "Полная полировка кузова",
            Price = 2500
        };

        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(service);

        var result = await _controller.GetById(1);
        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var returned = Assert.IsType<Service>(ok.Value);
        Assert.Equal("Полировка", returned.Name);
        Assert.Equal("Полная полировка кузова", returned.Opisanie);
        Assert.Equal(2500, returned.Price);
    }

    [Fact]
    public async Task Create_Returns_Created_Service()
    {
        var newService = new Service
        {
            Id = 1,
            Name = "Замена масла",
            Opisanie = "Полная замена масла",
            Price = 1500
        };

        _mockService.Setup(s => s.CreateAsync(newService)).ReturnsAsync(newService);

        var result = await _controller.Create(newService);
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);

        var returned = Assert.IsType<Service>(created.Value);
        Assert.Equal(1500, returned.Price);
    }

    [Fact]
    public async Task GetAll_Returns_Service_List()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Service>
        {
            new Service { Id = 1, Name = "A", Price = 100 },
            new Service { Id = 2, Name = "Б", Price = 200 },
        });

        var result = await _controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result.Result);

        var list = Assert.IsType<List<Service>>(ok.Value);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task GetById_Returns_Not_Found_When_Missing()
    {
        _mockService.Setup(s => s.GetByIdAsync(55)).ReturnsAsync((Service?)null);

        var result = await _controller.GetById(55);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_Returns_BadRequest_When_Id_Mismatch()
    {
        var service = new Service { Id = 3 };

        var result = await _controller.Update(5, service);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_Returns_NotFound_When_Service_Not_Exists()
    {
        _mockService.Setup(s => s.DeleteAsync(10)).ReturnsAsync(false);

        var result = await _controller.Delete(10);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
