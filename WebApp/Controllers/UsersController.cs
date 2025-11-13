using Microsoft.AspNetCore.Mvc;
using WebApp.Application.Interfaces;
using WebApp.models;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Пользователь ID {UserId} не найден", id);
                return NotFound(new { message = "Пользователь не найден" });
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            var created = await _userService.CreateAsync(user);
            _logger.LogInformation("Создан пользователь ID {UserId}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User updated)
        {
            if (id != updated.Id)
                return BadRequest(new { message = "ID не совпадает" });

            var success = await _userService.UpdateAsync(updated);
            if (!success)
            {
                _logger.LogWarning("Не удалось обновить пользователя ID {UserId}", id);
                return NotFound(new { message = "Пользователь не найден" });
            }

            _logger.LogInformation("Обновлен пользователь ID {UserId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Попытка удалить несуществующего пользователя ID {UserId}", id);
                return NotFound(new { message = "Пользователь не найден" });
            }

            _logger.LogInformation("Удален пользователь ID {UserId}", id);
            return NoContent();
        }
    }
}
