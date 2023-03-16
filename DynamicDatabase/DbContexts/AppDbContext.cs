using Microsoft.EntityFrameworkCore;

namespace DynamicDatabase.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
