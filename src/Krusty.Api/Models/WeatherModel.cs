using System.Text.Json.Serialization;

namespace Krusty.Api.Models
{
    public sealed class WeatherInfo
    {
        [JsonInclude]
        public double Temp { get; private set; }
        public double TempCelsius
        {
            get
            {
                var kelvinToCelsius = Temp - 273.15;
                return Math.Round(kelvinToCelsius, 1);
            }
        }
    }

    public sealed class WeatherModel
    {
        [JsonInclude]
        public WeatherInfo Main { get; private set; } = default!;
        public GeoModel Geo { get; private set; } = default!;
        public DateTime RequestDate { get; private set; }

        public WeatherModel()
        {
            RequestDate = DateTime.Now;
        }

        public WeatherModel WithLocation(GeoModel location)
        {
            Geo = location;
            return this;
        }
    }
}
