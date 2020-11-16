using Microsoft.EntityFrameworkCore;

namespace CarStore.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
