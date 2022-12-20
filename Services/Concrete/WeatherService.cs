using jorgen.ApplicationSettings;
using jorgen.Models.Domain;
using jorgen.Models.WeatherApiObject;
using jorgen.Services.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace jorgen.Services.Concrete
{
    public class WeatherService : IWeatherService
    {
        private static readonly HttpClient client = new();
        private readonly IOptionsMonitor<WeatherOptions> _options;

        public WeatherService(IOptionsMonitor<WeatherOptions> options)
        {
            _options = options;
        }

        public async Task<Weather> GetWeatherDataAsync()
        {
            string url = CreateWeatherApiUrl();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                
                var body = await response.Content.ReadAsStringAsync();

                WeatherModel.Root info = JsonConvert.DeserializeObject<WeatherModel.Root>(body);

                Weather weatherObject = new()
                {
                    Temp = double.Parse((info.Main.Temp - 273.15).ToString("F1")),
                    Humidity = info.Main.Humidity,
                    Pressure = info.Main.Pressure,
                    Speed = info.Wind.Speed,
                    Main = info.Weather.Select(x => x.Main).FirstOrDefault() ?? null
                };

                return weatherObject;
            }
        }

        private string CreateWeatherApiUrl()
        {
            StringBuilder sb = new();
            sb.Append(_options.CurrentValue.URL); // Base url
            sb.Append("veberod"); // City
            sb.Append("&appid="); // Binder
            sb.Append(_options.CurrentValue.ApiKey); // API Key

            return sb.ToString();
        }
    }
}
