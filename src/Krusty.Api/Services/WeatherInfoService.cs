using Krusty.Api.Extensions;
using Krusty.Api.Infrastructure;
using Krusty.Api.Models;

namespace Krusty.Api.Services
{
    internal sealed class WeatherInfoService : IWeatherInfoService
    {
        public IReadOnlyList<WeatherInfoResponse> GetWeatherInfoResponses(string cityName, DateTime beginDate, DateTime endDate)
        {
            var elements = MemoryContext<WeatherModel>.GetElements(
                s => string.Equals(s.Geo.Location, cityName, StringComparison.InvariantCultureIgnoreCase)
                && s.RequestDate >= beginDate
                && s.RequestDate <= endDate);

            return elements.ToWeatherInfoResponseList().AsReadOnly();
        }
    }

    internal static class WeatherInfoExtensions
    {
        internal static List<WeatherInfoResponse> ToWeatherInfoResponseList(this IEnumerable<WeatherModel> elements)
        {
            return [.. elements.Select(s =>
            new WeatherInfoResponse
            {
                City = s.Geo.Location,
                Temperature = $"{s.Main.TempCelsius}°C",
                Date = s.RequestDate.ToString("dd/MM/yyyy HH:ss")
            })];
        }
    }
}
