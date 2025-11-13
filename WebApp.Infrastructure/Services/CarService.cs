using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Application.Interfaces;
using WebApp.models;

namespace WebApp.Infrastructure.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CarService> _logger;

        public CarService(ApplicationContext context, ILogger<CarService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            var cars = await _context.Cars.Include(c => c.User).AsNoTracking().ToListAsync();
            _logger.LogInformation("Получено {Count} автомобилей", cars.Count);
            return cars;
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            var car = await _context.Cars.Include(c => c.User).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
                _logger.LogWarning("Автомобиль с ID {CarId} ненайден", id);
            return car;
        }

        public async Task<Car> CreateAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Создан автомобиль ID {CarId}", car.Id);
            return car;
        }

        public async Task<bool> UpdateAsync(Car car)
        {
            var existing = await _context.Cars.FindAsync(car.Id);
            if (existing == null) return false;

            existing.Marka = car.Marka;
            existing.Model = car.Model;
            existing.Number = car.Number;
            existing.UserId = car.UserId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Обновлен автомобиль ID {CarId}", car.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            _logger.LogWarning("Удален автомобиль ID {CarId}", id);
            return true;
        }
    }
}
