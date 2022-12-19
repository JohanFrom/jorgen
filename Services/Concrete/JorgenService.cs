using jorgen.Models.WeatherApiObject;
using jorgen.Services.Abstract;
using Newtonsoft.Json;

namespace jorgen.Services.Concrete
{
    public class JorgenService : IJorgenService
    {
        public string GetBeardStatus(double temp)
        {
            return JsonConvert.SerializeObject(WeatherModel.WeatherObject.CalculateJorgensBeard(temp));
        }

        public byte[] GetJorgenImage()
        {
            string? imagepathName = AppDomain.CurrentDomain.BaseDirectory + "data\\jorgenimg.png" ?? null;

            byte[] bytes = File.ReadAllBytes(imagepathName!);

            return bytes;
        }
    }
}
