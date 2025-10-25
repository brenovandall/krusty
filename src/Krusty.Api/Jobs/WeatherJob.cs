using Krusty.Api.Extensions;
using Krusty.Api.Factory;
using Krusty.Api.Infrastructure;
using Krusty.Api.Models;

namespace Krusty.Api.Jobs
{
    internal sealed class WeatherJob : IJobStrategy
    {
        public string Provider => JobProviders.WeatherConsuming;

        private readonly IConfiguration _configuration;
        private readonly IHttpService _httpService;

        public WeatherJob(IConfiguration configuration, IHttpService httpService)
        {
            _configuration = configuration;
            _httpService = httpService;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var brasiliaLocation = new GeoModel(Location: "Brasília", Lat: -15.7934036, Lon: -47.8823172);
            var portoAlegreLocation = new GeoModel(Location: "Porto Alegre", Lat: -30.0324999, Lon: -51.2303767);
            var curitibaLocation = new GeoModel(Location: "Curitiba", Lat: -25.4295963, Lon: -49.2712724);

            foreach (var location in new[] { brasiliaLocation, portoAlegreLocation, curitibaLocation })
            {
                try
                {
                    var weather = await GetWeather(location, cancellationToken);

                    PersistWeather(weather);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private static void PersistWeather(WeatherModel weather) => MemoryContext<WeatherModel>.AddElement(weather);

        private async Task<WeatherModel> GetWeather(GeoModel location, CancellationToken cancellationToken)
        {
            var apiKey = _configuration["OpenWeather:ApiKey"];
            var result = await _httpService.GetAsync<WeatherModel>(
                $"https://api.openweathermap.org/data/2.5/weather?lat={location.Lat}&lon={location.Lon}&appid={apiKey}", cancellationToken);

            return result.WithLocation(location);
        }
    }
}
