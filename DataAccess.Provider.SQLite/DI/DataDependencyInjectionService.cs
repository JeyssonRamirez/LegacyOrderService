using Core.GlobalRepository;
using DataAccess.Provider.SQLite.LocalKioskDBContext;
using DataAccess.Provider.SQLite.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Provider.SQLite.DI
{
    public static class DataDependencyInjectionService
    {
        public static IServiceCollection AddSQLiteData(this IServiceCollection services, IConfiguration configuration)
        {
            // EF Core with SQLite
            services.AddDbContext<LocalDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            //services.AddDbContext<LocalKioskDbContext>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
