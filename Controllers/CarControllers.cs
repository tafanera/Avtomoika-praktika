using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ApplicationContext _db;

        public CarController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            var cars = await _db.Cars
                .AsNoTracking()
                .Include(c => c.User)
                .ToListAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetById(int id)
        {
            var car = await _db.Cars
                .AsNoTracking()
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
                return NotFound(new { message = "Автомобиль не найден" });
            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Create(Car car)
        {
            var userExists = await _db.Users.AnyAsync(c => c.Id == car.UserId);
            if (!userExists)
                return BadRequest(new { message = $"Клиент {car.UserId} не найден" });
            _db.Cars.Add(car);
            await _db.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Car updated)
        {
            if (id != updated.Id)
                return BadRequest();
            
            var car = await _db.Cars.FindAsync(id);
            if (car == null)
                return NotFound(new {message = "Автомобиль не найден"});
            car.Marka = updated.Marka;
            car.Model = updated.Model;
            car.Number = updated.Number;
            car.UserId = updated.UserId;
            
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car =  await _db.Cars.FindAsync(id);
            if (car == null)
                return NotFound(new {message = "Автомобиль не найден"});
            
            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
    
}
    

