namespace Krusty.Api.Factory
{
    public interface IJobStrategy
    {
        string Provider { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
