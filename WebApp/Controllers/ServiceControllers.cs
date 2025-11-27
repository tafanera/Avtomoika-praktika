using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Interfaces;
using WebApp.models;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceService serviceService, ILogger<ServiceController> logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetAll()
        {
            var services = await _serviceService.GetAllAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
            {
                _logger.LogWarning("Услуга ID {ServiceId} не найдена", id);
                return NotFound(new { message = "Услуга не была найдена" });
            }

            return Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<Service>> Create(Service service)
        {
            var created = await _serviceService.CreateAsync(service);
            _logger.LogInformation("Создана услуга ID {ServiceId}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Service updated)
        {
            if (id != updated.Id)
                return BadRequest(new { message = "ID не совпадает" });

            var success = await _serviceService.UpdateAsync(updated);
            if (!success)
            {
                _logger.LogWarning("Не удалось обновить услугу ID {ServiceId}", id);
                return NotFound(new { message = "Услуга не найдена" });
            }

            _logger.LogInformation("Обновлена услуга ID {ServiceId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _serviceService.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Попытка удалить несуществующую услугу ID {ServiceId}", id);
                return NotFound(new { message = "Услуга не найдена" });
            }

            _logger.LogInformation("Удалена услуга ID {ServiceId}", id);
            return NoContent();
        }
    }
}
