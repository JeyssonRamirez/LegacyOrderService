using LegacyOrderService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Provider.SQLite.LocalKioskDBContext
{
    public class LocalKioskDbContext : DbContext
    {
        public LocalKioskDbContext(DbContextOptions<LocalKioskDbContext> options)
        : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=orders.db;Mode=ReadWriteCreate", x => x.MigrationsAssembly("DataAccess.Provider.SQLite"))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);
        }
    }
}
