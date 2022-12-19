using jorgen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace jorgen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JorgenController : ControllerBase
    {
        private static readonly HttpClient client = new();
        private readonly string _url = "https://pinger-23654.azurewebsites.net/";
        private readonly ILogger<JorgenController> _logger;
        private readonly string _apiKey = "0c87245268ef262893e0da7caa3d6e37";

        public JorgenController(ILogger<JorgenController> logger, IConfiguration config)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetJorgenImage()
        {
            try
            {
                string? imagepathName = AppDomain.CurrentDomain.BaseDirectory + "data\\jorgenimg.png" ?? null;

                if (imagepathName == null)
                {
                    return Content("Could not find image");
                }

                byte[] b = System.IO.File.ReadAllBytes(imagepathName);

                return File(b, "image/png");
            }
            catch (Exception e)
            {
                return Content($"Could not return image.{Environment.NewLine}Error message: {e.Message} ");
            }
        }

        [HttpGet("ping/{sleepTime}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListenForPing(int sleepTime)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(_url + "Pinger/" + sleepTime);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return Content($"Could not listen for ping.{Environment.NewLine}Error message: {e.Message} ");
            }
        }

        [HttpGet("getweather")]
        public async Task<ActionResult> GetWeatherData()
        {
            try
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", "veberod", _apiKey);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url)
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    if (!response.IsSuccessStatusCode)
                    {
                        return Content("Could not fetch the weather api");
                    }


                    var body = await response.Content.ReadAsStringAsync();

                    WeatherModel.Root info = JsonConvert.DeserializeObject<WeatherModel.Root>(body);

                    WeatherModel.WeatherObject weatherObject = new()
                    {
                        Temp = double.Parse((info.Main.Temp - 273.15).ToString("F2")),
                        Humidity = info.Main.Humidity,
                        Pressure = info.Main.Pressure,
                        Speed = info.Wind.Speed,
                        Main = info.Weather.Select(x => x.Main).FirstOrDefault() ?? null
                    };
                    return Ok(weatherObject);

                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception message: " + e.Message);
            }
        }
        [HttpGet("testweater")]
        public async Task<ActionResult<object>> GetTestWeather()
        {
            string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", "veberod", _apiKey);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            using(var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    return Content("Could not fetch the weather api");
                }


                var body = await response.Content.ReadAsStringAsync();

                return body; 

            }

        }

        [HttpGet("statusOfBeard")]
        public ActionResult GetJorgenStatusBeard(double temp)
        {            
            string s = JsonConvert.SerializeObject(WeatherModel.WeatherObject.CalculateJorgensBeard(temp));

            return Ok(s);
        }
    }
}
