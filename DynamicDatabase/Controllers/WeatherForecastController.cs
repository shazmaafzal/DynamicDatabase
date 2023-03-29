using DynamicDatabase.DbContexts;
using DynamicDatabase.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

namespace DynamicDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly AppDbContext _context;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbContextFactory<AppDbContext> dbContextFactory, IConfiguration _configuration, AppDbContext context)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _context = context;
            Configuration = _configuration;
        }

        [HttpPost("{databaseName}")]
        public async Task<IActionResult> CreateDatabase(string databaseName)
        {
            string test = HtmlEncoder.Default.Encode(databaseName);
            //string test = System.Web.HttpUtility.UrlEncode(databaseName);
            await _context.Database.ExecuteSqlRawAsync($"CREATE DATABASE {test}");
            return StatusCode(StatusCodes.Status201Created, $"Database {databaseName} Created!");
        }

        [HttpPost]
        public IActionResult CreateDatabase()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Database.EnsureCreated();
            }

            return Ok("Database created successfully");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get(string server)
        {
            string connStr = Configuration.GetConnectionString("DynamicConnection");
            connStr = string.Format(connStr, server);
            return Ok(this.GetCustomer(connStr));
        }

        private List<Customer> GetCustomer(string connectionString)
        {
            CustomerDbContext _contextFactory = new CustomerDbContext(connectionString);
            return _contextFactory.Customer.ToList();
        }
    }
}