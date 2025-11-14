using Core.GlobalRepository;
using DataAccess.Provider.SQLite.LocalKioskDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return services;
        }
    }
}
