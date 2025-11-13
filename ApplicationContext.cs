using Microsoft.EntityFrameworkCore;
namespace WebApp.models;


public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Вован", Age = 19 },
            new User { Id = 2, Name = "Робчик", Age = 30 },
            new User { Id = 3, Name = "Антон", Age = 24 }
        );
    }



}
    
    