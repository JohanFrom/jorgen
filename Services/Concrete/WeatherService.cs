using jorgen.ApplicationSettings;
using jorgen.Models.Domain;
using jorgen.Models.WeatherApiObject;
using jorgen.Services.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace jorgen.Services.Concrete
{
    public class WeatherService : IWeatherService
    {
        private static readonly HttpClient client = new();
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<AdminOptions> _options;
        public WeatherService(IConfiguration configuration, IOptionsMonitor<AdminOptions> options)
        {
            _configuration = configuration;
            _options = options;
        }

        public async Task<Weather> GetWeatherDataAsync()
        {
            var testar = _options.CurrentValue.AdministratorUserIds.Where(x => x == "SEFROMJ").First();
            string apiKey = _configuration.GetSection("WeatherApi:WeatherApiKey").Value;
            string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", "veberod", apiKey);
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
                    return null;
                }

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
    }
}
