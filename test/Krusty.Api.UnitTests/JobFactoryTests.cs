using Krusty.Api.Factory;

namespace Krusty.Api.UnitTests
{
    public class JobFactoryTests
    {
        private readonly JobFactory _target;

        public JobFactoryTests()
        {
            _target = new JobFactory([new FakeJobStrategy()]);
        }

        [Fact]
        public void Create_WhenValidProvider_ShouldReturnJobStrategyInstance()
        {
            var strategy = _target.Create("FakeJob");

            Assert.NotNull(strategy);
            Assert.True(strategy.Provider == "FakeJob");
        }

        [Fact]
        public void Create_WhenInvalidProvider_ShouldThrow()
        {
            Assert.Throws<Exception>(() => _target.Create(""));
        }

        [Fact]
        public async Task Create_WhenValidProvider_ShouldReturnJobStrategyInstanceThatCanBeExecuted()
        {
            // conversao explicita para nao comprometer Assert.NotNull
            var strategy = _target.Create("FakeJob") as FakeJobStrategy;

            Assert.NotNull(strategy);
            Assert.True(strategy.Provider == "FakeJob");

            await strategy.ExecuteAsync(CancellationToken.None);

            Assert.True(strategy.Success);
        }
    }

    internal class FakeJobStrategy : IJobStrategy
    {
        public string Provider => "FakeJob";

        public bool Success { get; private set; }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Success = true;
            return Task.CompletedTask;
        }
    }
}
