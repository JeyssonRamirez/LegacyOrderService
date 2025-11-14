using LegacyOrderService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Provider.SQLite.LocalKioskDBContext
{
    public class LocalDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public LocalDbContext(DbContextOptions<LocalDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasAnnotation("Sqlite:Autoincrement", true);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=orders.db;Mode=ReadWriteCreate", x => x.MigrationsAssembly("DataAccess.Provider.SQLite"))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);
        }
    }
}
