using jorgen.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace jorgen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("getweather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetWeatherData()
        {
            var weatherData = await _weatherService.GetWeatherDataAsync();

            if(weatherData == null)
            {
                return NotFound();
            }

            return Ok(weatherData);
            
        }
    }
}