using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
    
        public UsersController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _db.Users
                .Include(c => c.Cars)
                .Include(c => c.Order)
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] User updated)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return NotFound(new {message = $"Пользователь с ID {id} не найден"});
        
            existingUser.Name = updated.Name;
            existingUser.Number = updated.Number;

            try
            {
                _db.SaveChanges();
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при сохранении изменений: " + ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user =  _db.Users.FirstOrDefault(u => u.Id == id);
        
            _db.Users.Remove(user);
            _db.SaveChanges();
            return NoContent();
        }
    }  
}



