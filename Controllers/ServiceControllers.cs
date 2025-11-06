using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly ApplicationContext _db;

        public ServiceController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> Get()
        {
            var services = await _db.Services
                .AsNoTracking()
                .ToListAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetById(int id)
        {
            var service = await _db.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
                return NotFound(new { Message = "Услуга не найдена" });
            return Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<Service>> Create(Service service)
        {
            _db.Services.Add(service);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = service.Id }, service);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> Update(int id, Service updated)
        {
            if (id != updated.Id)
                return BadRequest();
            var service = await _db.Services.FindAsync(id);
            if (service == null)
                return NotFound(new {message = "Услуга не найдена"});
            
            service.Name = updated.Name;
            service.Opisanie = updated.Opisanie;
            service.Price = updated.Price;
            
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var service = await _db.Services.FindAsync(id);
            if  (service == null)
                return NotFound(new {message = "Услуга не найдена"});
            
            _db.Services.Remove(service);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}