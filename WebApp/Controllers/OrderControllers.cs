using Microsoft.AspNetCore.Mvc;
using WebApp.Application;
using WebApp.Application.Interfaces;
using WebApp.models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Заказ ID {OrderId} не найден", id);
                return NotFound(new { message = "Заказ не был найден" });
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderDto dto)
        {
            var created = await _orderService.CreateAsync(dto);
            _logger.LogInformation("Создан заказ ID {OrderId}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order updated)
        {
            if (id != updated.Id)
                return BadRequest(new { message = "ID не совпадает" });

            var success = await _orderService.UpdateAsync(updated);
            if (!success)
            {
                _logger.LogWarning("Не удалось обновить заказ ID {OrderId}", id);
                return NotFound(new { message = "Заказ не найден" });
            }

            _logger.LogInformation("Обновлен заказ ID {OrderId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderService.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Попытка удалить несуществующий заказ ID {OrderId}", id);
                return NotFound(new { message = "Заказ не найден" });
            }

            _logger.LogInformation("Удален заказ ID {OrderId}", id);
            return NoContent();
        }
    }
}
