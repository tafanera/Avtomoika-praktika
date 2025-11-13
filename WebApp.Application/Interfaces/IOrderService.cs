using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.models;

namespace WebApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order> CreateAsync(OrderDto orderDto);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
    }
}