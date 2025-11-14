using LegacyOrderService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Provider.SQLite.LocalKioskDBContext
{
    public class LocalDbContext : DbContext
    {

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
        : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasAnnotation("Sqlite:Autoincrement", true);

            // Seeding Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Widget", Price = 12.99m, Status = StatusType.Active, RegistrationDate = DateTime.UtcNow },
                new Product { Id = 2, Name = "Gadget", Price = 15.49m, Status = StatusType.Active, RegistrationDate = DateTime.UtcNow },
                new Product { Id = 3, Name = "Doohickey", Price = 8.75m, Status = StatusType.Inactive, RegistrationDate = DateTime.UtcNow }
            );


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
