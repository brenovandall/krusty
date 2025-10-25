namespace Krusty.Api.Factory
{
    public interface IJobFactory
    {
        IJobStrategy Create(string provider);
    }
}
