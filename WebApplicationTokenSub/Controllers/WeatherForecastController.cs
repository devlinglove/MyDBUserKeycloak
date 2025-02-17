using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationTokenSub.Options;
using WebApplicationTokenSub.Services;

namespace WebApplicationTokenSub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CreateUserService _createUserService;
        private readonly ILogger<WeatherForecastController> _logger;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

       

        public WeatherForecastController(ILogger<WeatherForecastController> logger, CreateUserService createUserService)
        {
            _logger = logger;
            _createUserService = createUserService;
        }


        [Authorize]
        [Route("GetWeatherForecast2")]
        [HttpGet]
        public async Task<IActionResult> Get2()
        {
            Console.WriteLine($"token-sub-parameter-{User.FindFirstValue(ClaimTypes.NameIdentifier)}");
            Console.WriteLine($"token-sub-role-{User.FindFirstValue(ClaimTypes.Role)}");

            var content = await _createUserService.GetUserAsync();

            return Ok(content);
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine($"token-sub-parameter-{User.FindFirstValue(ClaimTypes.NameIdentifier)}");
            Console.WriteLine($"token-sub-role-{User.FindFirstValue(ClaimTypes.Role)}");

            

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
