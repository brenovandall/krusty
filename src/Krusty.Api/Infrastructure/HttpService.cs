using System.Text.Json;

namespace Krusty.Api.Infrastructure
{
    internal sealed class HttpService : IHttpService
    {
        public async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            var client = new HttpClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            var response = await client.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception(errorMsg);
            }

            return await DeserializeAsync<T>(response, cancellationToken);
        }

        private async Task<T> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var data = await response.Content.ReadAsStringAsync(cancellationToken);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(data, options)!;
        }
    }
}
