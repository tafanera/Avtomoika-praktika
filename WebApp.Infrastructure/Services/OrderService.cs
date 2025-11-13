using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Application;
using WebApp.Application.Interfaces;
using WebApp.models;


namespace WebApp.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Cars)
                .Include(o => o.Services)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation("Получено {Count} заказов", orders.Count);
            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Cars)
                .Include(o => o.Services)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                _logger.LogWarning("Заказ с ID {OrderId} не найден", id);

            return order;
        }

        public async Task<Order> CreateAsync(OrderDto dto)
        {
            var services = await _context.Services.Where(s => dto.ServiceIds.Contains(s.Id)).ToListAsync();
            var order = new Order
            {
                UserId = dto.BuyerId,
                CarId = dto.CarId,
                OrderDate = dto.OrderDate,
                Status = dto.Status,
                Services = services,
                TotalPrice = services.Sum(s => s.Price)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Создан новый заказ ID {OrderId}", order.Id);
            return order;
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            var existing = await _context.Orders.FindAsync(order.Id);
            if (existing == null) return false;

            existing.Status = order.Status;
            existing.TotalPrice = order.TotalPrice;
            existing.OrderDate = order.OrderDate;
            existing.CarId = order.CarId;
            existing.UserId = order.UserId;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Обновлен заказ ID {OrderId}", order.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            _logger.LogWarning("Удален заказ ID {OrderId}", id);
            return true;
        }
    }
}
