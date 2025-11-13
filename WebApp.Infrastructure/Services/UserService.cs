using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Application.Interfaces;
using WebApp.models;
using WebApp.models;

namespace WebApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            _logger.LogInformation("Получен список всех пользователей");
            return await _db.Users
                .Include(u => u.Cars)
                .Include(u => u.Order)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users
                .Include(u => u.Cars)
                .Include(u => u.Order)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Добавлен новый пользователь {Name}", user.Name);
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            if (!await _db.Users.AnyAsync(u => u.Id == user.Id))
                return false;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Пользователь обновлен {Id}", user.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Удален пользователь {Id}", id);
            return true;
        }

    }
}