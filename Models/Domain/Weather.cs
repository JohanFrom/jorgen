namespace jorgen.Models.Domain
{
    public class Weather
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double Speed { get; set; }
        public string? Main { get; set; }
    }
}
