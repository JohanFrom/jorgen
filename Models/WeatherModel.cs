namespace jorgen.Models
{
    class WeatherModel
    {
        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Weather
        {
            public string main { get; set; }
            public string description { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double pressure { get; set; }
            public double humidity { get; set; }

        }

        public class Wind
        {
            public double speed { get; set; }
        }

        public class Sys
        {
            public long sunrise { get; set; }
            public long sunset { get; set; }
        }

        public class Root
        {
            public Coord coord { get; set; }
            public List<Weather> weather { get; set; }
            public Main main { get; set; }

            public Wind wind { get; set; }
            public Sys sys { get; set; }
        }



        public class WeatherObject
        {
            public double temp { get; set; }
            public double pressure { get; set; }
            public double humidity { get; set; }
            public double speed { get; set; }

            public string? main { get; set; }
        }
    }
}
