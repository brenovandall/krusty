namespace Krusty.Api.Infrastructure
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken);
    }
}
