using DynamicDatabase.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly AppDbContext _context;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbContextFactory<AppDbContext> dbContextFactory, AppDbContext context)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _context = context;
        }

        [HttpPost("{databaseName}")]
        public async Task<IActionResult> CreateDatabase(string databaseName)
        {
            await _context.Database.ExecuteSqlRawAsync($"CREATE DATABASE {databaseName}");
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
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}