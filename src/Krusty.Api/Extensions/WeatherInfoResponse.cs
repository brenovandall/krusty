namespace Krusty.Api.Extensions
{
    public sealed class WeatherInfoResponse
    {
        public string City { get; set; } = default!;
        public string Temperature { get; set; } = default!;
        public string Date { get; set; } = default!;
    }
}
