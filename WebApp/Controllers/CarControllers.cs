using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Interfaces;
using WebApp.models;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly ILogger<CarController> _logger;

        public CarController(ICarService carService, ILogger<CarController> logger)
        {
            _carService = carService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAll()
        {
            var cars = await _carService.GetAllAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetById(int id)
        {
            var car = await _carService.GetByIdAsync(id);
            if (car == null)
            {
                _logger.LogWarning("Попытка получить несуществующий автомобиль с ID {CarId}", id);
                return NotFound(new { message = "Автомобиль не был найден" });
            }

            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Create(Car car)
        {
            var created = await _carService.CreateAsync(car);
            _logger.LogInformation("Создан автомобиль с ID {CarId}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Car updated)
        {
            if (id != updated.Id)
                return BadRequest(new { message = "ID не совпадает" });

            var result = await _carService.UpdateAsync(updated);
            if (!result)
            {
                _logger.LogWarning("Не удалось обновить автомобиль ID {CarId}", id);
                return NotFound(new { message = "Автомобиль не найден" });
            }

            _logger.LogInformation("Обновлен автомобиль ID {CarId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _carService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning("Попытка удалить несуществующий автомобиль ID {CarId}", id);
                return NotFound(new { message = "Автомобиль не найден" });
            }

            _logger.LogInformation("Удален автомобиль ID {CarId}", id);
            return NoContent();
        }
    }
}
