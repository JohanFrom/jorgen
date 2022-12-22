﻿using jorgen.ApplicationSettings;
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
        private readonly IOptionsMonitor<ApplicationOptions> _options;
        private readonly IKeyVaultService _keyVaultService;

        public WeatherService(IOptionsMonitor<ApplicationOptions> options, IKeyVaultService keyVaultService)
        {
            _options = options;
            _keyVaultService = keyVaultService;
        }

        public async Task<Weather?> GetWeatherDataAsync(string city)
        {
            string url = CreateWeatherApiUrl(city);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };

            using (var response = await client.SendAsync(request))
            {
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

        private string CreateWeatherApiUrl(string city)
        {
            StringBuilder sb = new();
            sb.Append(_options.CurrentValue.WeatherUrl); // Base url
            sb.Append(city); // City
            sb.Append("&appid="); // City - Binder - Apikey
            sb.Append(_keyVaultService.GetWeatherApiKey()); // API Key

            return sb.ToString();
        }
    }
}
