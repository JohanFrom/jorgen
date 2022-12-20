﻿using jorgen.Models.Domain;
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

        /// <summary>
        /// Retrieves the current weather data of a city
        /// </summary>
        /// <param name="city">English characters a-z</param>
        /// <returns>Weather data object</returns>
        [HttpGet("getweather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Weather>> GetWeatherData(string city)
        {
            var weatherData = await _weatherService.GetWeatherDataAsync(city);

            if(weatherData == null)
            {
                return NotFound();
            }

            return Ok(weatherData);
            
        }
    }
}