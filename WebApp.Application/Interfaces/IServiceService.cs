using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Application.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task<Service> CreateAsync(Service service);
        Task<bool> UpdateAsync(Service service);
        Task<bool> DeleteAsync(int id);
    }
}