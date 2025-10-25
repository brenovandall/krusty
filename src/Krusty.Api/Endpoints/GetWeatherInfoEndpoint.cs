using Krusty.Api.Models;
using Carter;
using Krusty.Api.Services;

namespace Krusty.Api.Endpoints
{
    public sealed class GetWeatherInfoEndpoint : ICarterModule
    {
        public sealed record GetWeatherInfoQueryParams(string CityName, DateTime BeginDate, DateTime EndDate);
        public sealed record GetWeatherInfoResult(IReadOnlyList<WeatherInfo> WeatherInfos);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("v1/api/weather", (
                [AsParameters] GetWeatherInfoQueryParams query,
                IWeatherInfoService weatherInfoService,
                CancellationToken cancelationToken) =>
            {
                var result = weatherInfoService.GetWeatherInfoResponses(query.CityName, query.BeginDate, query.EndDate);

                return Results.Ok(result);
            })
            .WithName("GetWeatherInfo")
            .Produces<GetWeatherInfoResult>(StatusCodes.Status200OK)
            .WithSummary("Obter informações meteorológicas filtradas por cidade e intervalo de datas.")
            .WithDescription(@"Este endpoint retorna as informações meteorológicas armazenadas em memória para uma cidade específica dentro de um intervalo de datas informado.

### Detalhes:
- A consulta é realizada no contexto de memória (`MemoryContext<WeatherModel>`), retornando apenas os registros que correspondem ao nome da cidade (`CityName`) e cujas datas de requisição (`RequestDate`) estejam entre os parâmetros `BeginDate` e `EndDate`.
- O resultado é retornado no formato JSON contendo uma lista de objetos meteorológicos (`WeatherInfo`).

### Parâmetros de consulta (`query params`):
- **cityName** *(string, obrigatório)* — Nome da cidade para a busca.
- **beginDate** *(DateTime, obrigatório)* — Data inicial do intervalo de busca (formato esperado: `yyyy-MM-dd` ou `yyyy-MM-ddTHH:mm:ss`).
- **endDate** *(DateTime, obrigatório)* — Data final do intervalo de busca (formato esperado: `yyyy-MM-dd` ou `yyyy-MM-ddTHH:mm:ss`).

### Retorno:
- **200 OK** — Retorna uma lista de informações meteorológicas que atendem aos filtros especificados.
- **Exemplo de resposta:**
```json
[
  {
    ""city"": ""Curitiba"",
    ""temperature"": ""23.8°C"",
    ""date"": ""04/10/2025 12:20""
  }
]");
        }
    }
}
