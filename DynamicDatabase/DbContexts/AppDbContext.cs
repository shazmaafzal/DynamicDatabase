using Microsoft.EntityFrameworkCore;

namespace DynamicDatabase.DbContexts
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MyEntity> MyEntities { get; set; }
    }
}
