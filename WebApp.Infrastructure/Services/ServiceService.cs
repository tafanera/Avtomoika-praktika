using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Application.Interfaces;
using WebApp.models;


namespace WebApp.Infrastructure.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ServiceService> _logger;

        public ServiceService(ApplicationContext context, ILogger<ServiceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            var services = await _context.Services.AsNoTracking().ToListAsync();
            _logger.LogInformation("Получено {Count} услуг", services.Count);
            return services;
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
                _logger.LogWarning("Услуга ID {ServiceId} не найдена", id);
            return service;
        }

        public async Task<Service> CreateAsync(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Создана услуга ID {ServiceId}", service.Id);
            return service;
        }

        public async Task<bool> UpdateAsync(Service service)
        {
            var existing = await _context.Services.FindAsync(service.Id);
            if (existing == null) return false;

            existing.Name = service.Name;
            existing.Opisanie = service.Opisanie;
            existing.Price = service.Price;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Обновлена услуга ID {ServiceId}", service.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            _logger.LogWarning("Удалена услуга ID {ServiceId}", id);
            return true;
        }
    }
}
