using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DynamicDatabase.DbContexts
{
    public class DynamicDbContextFactory : IDbContextFactory<AppDbContext>
    {
        private readonly IConfiguration _configuration;

        public DynamicDbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AppDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = GetConnectionStringFromSomewhere();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }

        private string GetConnectionStringFromSomewhere()
        {
            var dynamicConnectionString = _configuration.GetConnectionString("DynamicConnection");
            return dynamicConnectionString ?? string.Empty;
        }
    }
}
