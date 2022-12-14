using System.Text;

namespace jorgen.Models
{
    class WeatherModel
    {
        public class Coord
        {
            public double Lon { get; set; }
            public double Lat { get; set; }
        }

        public class Weather
        {
            public string Main { get; set; }
            public string Description { get; set; }
        }

        public class Main
        {
            public double Temp { get; set; }
            public double Pressure { get; set; }
            public double Humidity { get; set; }

        }

        public class Wind
        {
            public double Speed { get; set; }
        }

        public class Sys
        {
            public long Sunrise { get; set; }
            public long Sunset { get; set; }
        }

        public class Root
        {
            public Coord Coord { get; set; }
            public List<Weather> Weather { get; set; }
            public Main Main { get; set; }

            public Wind Wind { get; set; }
            public Sys Sys { get; set; }
        }



        public class WeatherObject
        {
            public double Temp { get; set; }
            public double Pressure { get; set; }
            public double Humidity { get; set; }
            public double Speed { get; set; }
            public string? Main { get; set; }

            public static string CalculateJorgensBeard(double temp)
            {
                StringBuilder sb = new();
                if (temp < 0)
                {
                    sb.Append("Jörgens skägg är fruset och kyligt!");
                }
                else if (temp >= 0 && temp <= 4)
                {
                    sb.Append("Jörgens skäggg är vått och kallt!");
                }
                else if (temp >= 5 && temp <= 9)
                {
                    sb.Append("Jörgens skägg är smått kallt, på bättringsvägen!");
                }
                else if (temp >= 10 && temp <= 15)
                {
                    sb.Append("Jörgens skägg är för tillfället väldigt bekvämligt!");
                }
                else if (temp >= 15 && temp <= 20)
                {
                    sb.Append("Jörgens skägg är påväg att bli fuktigt!");
                }
                else if (temp >= 21)
                {
                    sb.Append("Jörgens skägg är väldigt fuktigt!");
                }

                return sb.ToString();
            }
        }
    }
}
