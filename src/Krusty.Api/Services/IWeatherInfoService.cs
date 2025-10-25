using Krusty.Api.Extensions;

namespace Krusty.Api.Services
{
    public interface IWeatherInfoService
    {
        IReadOnlyList<WeatherInfoResponse> GetWeatherInfoResponses(string cityName, DateTime beginDate, DateTime endDate);
    }
}
