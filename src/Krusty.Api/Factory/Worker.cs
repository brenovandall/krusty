namespace Krusty.Api.Factory
{
    internal sealed class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceFactory;
        private readonly string _strategy;

        public Worker(IServiceScopeFactory serviceFactory, string strategy)
        {
            _serviceFactory = serviceFactory;
            _strategy = strategy;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceFactory.CreateScope();
                var factory = scope.ServiceProvider.GetRequiredService<IJobFactory>();
                var strategy = factory.Create(_strategy);

                _ = Task.Run(() => strategy.ExecuteAsync(stoppingToken), stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
