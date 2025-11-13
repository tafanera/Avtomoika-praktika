using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.models;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllAsync();
        Task<Car?> GetByIdAsync(int id);
        Task<Car> CreateAsync(Car car);
        Task<bool> UpdateAsync(Car car);
        Task<bool> DeleteAsync(int id);
    }
}