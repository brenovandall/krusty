namespace Krusty.Api.Factory
{
    internal sealed class JobFactory : IJobFactory
    {
        private readonly IEnumerable<IJobStrategy> _strategies;

        public JobFactory(IEnumerable<IJobStrategy> strategies)  => _strategies = strategies;

        public IJobStrategy Create(string provider)
        {
            var strategy = _strategies.FirstOrDefault(s => s.Provider == provider);
            if (strategy == null) throw new Exception("Strategy não encontrada.");

            return strategy;
        }
    }
}
