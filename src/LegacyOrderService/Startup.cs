using Application.Definition.IServices;
using Application.Implementation.Services;
using DataAccess.Provider.SQLite.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LegacyOrderService
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {

            // Create a service collection
            var services = new ServiceCollection();

            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


            services.AddSingleton<IConfiguration>(configuration);

            services.AddSingleton<IOrderAppService, OrderAppService>();

            //Local DB 
            services.AddSQLiteData(configuration);

            // Build the service provider
            return services.BuildServiceProvider();
        }
    }
}
