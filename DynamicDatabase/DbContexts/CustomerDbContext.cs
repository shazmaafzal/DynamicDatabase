using DynamicDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace DynamicDatabase.DbContexts
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(string databaseConnection)
      : base()
        {
            ConnectionString = databaseConnection.ToString();
        }

        public string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true).UseSqlServer(ConnectionString);
        }
        public DbSet<Customer> Customer { get; set; }
    }
}
