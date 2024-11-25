using Microsoft.EntityFrameworkCore;

namespace netCore_Begginer.Models
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options) { }

        public ProductDbContext() { }

        public virtual DbSet<UserAuthentication> UserAuthentication { get; set; }

        public DbSet<DailyTasks> DailyTasks { get; set; } 

    }
}
