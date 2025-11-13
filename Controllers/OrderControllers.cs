using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Application;
using WebApp.models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ApplicationContext _db;

        public OrderController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orders = await _db.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.Cars)
                .Include(o => o.Services)
                .ToListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.Cars)
                .Include(o => o.Services)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound(new { message = "Заказ не найден" });

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Craete(OrderDto orderDto)
        {
            var services = await _db.Services
                .Where(s => orderDto.ServiceIds.Contains(s.Id))
                .ToListAsync<Service>();
            if (services.Count != orderDto.ServiceIds.Count)
                return BadRequest(new { message = "Услуги не найдены" });
            
            var order = new Order
            {
                UserId = orderDto.BuyerId,
                CarId = orderDto.CarId,
                OrderDate = orderDto.OrderDate,
                Status = orderDto.Status,
                Services = services,
                TotalPrice = services.Sum(s => s.Price)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order updated)
        {
            if ( id != updated.Id)
                return BadRequest(new { message = "Некорректный ID заказа" });

            var order = await _db.Orders.FindAsync(id);
            if (order == null)
                return NotFound(new { message = "Заказ не найден" });
            
            order.UserId = updated.UserId;
            order.CarId = updated.CarId;
            order.OrderDate = updated.OrderDate;
            order.Status = updated.Status;
            order.Services = updated.Services;
            order.TotalPrice = updated.TotalPrice;
            
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
                return NotFound(new {message = "Заказ не найден" });
            
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            
            return NoContent();
        }
        
    }
}