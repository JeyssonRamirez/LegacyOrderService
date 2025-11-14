using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DataAccess.Provider.SQLite.LocalKioskDBContext
{
    public class LocalDbContextFactory : IDesignTimeDbContextFactory<LocalDbContext>
    {
        
        public LocalDbContextFactory()
        {
            
            
        }
        public LocalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LocalDbContext>();
            optionsBuilder.UseSqlite("Data Source=orders.db");
            return new LocalDbContext(optionsBuilder.Options);
        }
    }
}
