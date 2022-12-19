
using jorgen.Models.Domain;

namespace jorgen.Services.Abstract
{
    public interface IWeatherService
    {
        public Task<Weather> GetWeatherDataAsync();
    }
}
