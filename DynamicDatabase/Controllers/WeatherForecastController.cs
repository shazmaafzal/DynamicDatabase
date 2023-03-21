using DynamicDatabase.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Xml;

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
        private readonly IConfiguration Configuration;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbContextFactory<AppDbContext> dbContextFactory, AppDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _context = context;
            Configuration = configuration;
        }

        [HttpPost("{databaseName}")]
        public async Task<IActionResult> CreateDatabase(string databaseName)
        {
            await _context.Database.ExecuteSqlRawAsync($"CREATE DATABASE {databaseName}");
            return StatusCode(StatusCodes.Status201Created, $"Database {databaseName} Created!");
        }

        [HttpPost]
        [Route("create-new-table")]
        public async Task<IActionResult> CreateNewTable([FromBody] string connectionString)
        {
            //string connectionString = Configuration.GetConnectionString("DefaultConnection");

            // Modify the DbContext options to include the connection string
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Create an instance of the DbContext using the modified options
            var dbContext = new AppDbContext(options);

            try
            {
                await dbContext.Database.EnsureCreatedAsync();
            }
            catch (Exception oops)
            {
                return BadRequest(oops.Message);
            }

            return StatusCode(StatusCodes.Status201Created);
        }



        //[HttpPost]
        //[Route("create-new-table")]
        //public async Task<IActionResult> CreateNewTable()
        //{
        //    // Ensure that the database and tables are created

        //    try
        //    {
        //        await _context.Database.EnsureCreatedAsync();
        //    }
        //    catch (Exception oops)
        //    {
        //        return BadRequest(oops.Message);
        //    }

        //    //// You can now use the MyEntities DbSet to perform CRUD operations
        //    //var newEntity = new MyEntity { Name = "New Entity" };
        //    //_context.MyEntities.Add(newEntity);
        //    //await _context.SaveChangesAsync();

        //    return StatusCode(StatusCodes.Status201Created);
        //}

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