using Krusty.Api.Infrastructure;
using Krusty.Api.Jobs;
using Krusty.Api.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Krusty.Api.UnitTests
{
    public class WeatherJobTests
    {
        private readonly Mock<IHttpService> _httpService;

        private readonly WeatherJob _target;

        public WeatherJobTests()
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(s => s["OpenWeather:ApiKey"]).Returns("key");
            _httpService = new Mock<IHttpService>();
            _target = new WeatherJob(configuration.Object, _httpService.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallHttpServiceForEachCity_AndPersistResults()
        {
            MemoryContext<WeatherModel>.ClearBucket();

            var fakeModel = new WeatherModel();
            _httpService
                .Setup(s => s.GetAsync<WeatherModel>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeModel);

            await _target.ExecuteAsync(CancellationToken.None);

            _httpService.Verify(s => s.GetAsync<WeatherModel>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(3));

            var elements = MemoryContext<WeatherModel>.GetElements();

            Assert.True(elements.Count() == 3);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldContinue_WhenHttpServiceThrows()
        {
            MemoryContext<WeatherModel>.ClearBucket();

            int index = 0;
            _httpService
            .Setup(s => s.GetAsync<WeatherModel>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((_, _) =>
            {
                index++;

                if (index == 2)
                    throw new Exception("testing");

                return Task.FromResult(new WeatherModel());
            });

            await _target.ExecuteAsync(CancellationToken.None);

            var elements = MemoryContext<WeatherModel>.GetElements();

            Assert.True(index == 3);
            Assert.True(elements.Count() == 2);
        }
    }
}
