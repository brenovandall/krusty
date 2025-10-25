using Krusty.Api.Extensions;
using Krusty.Api.Factory;
using Krusty.Api.Infrastructure;
using Krusty.Api.Jobs;
using Krusty.Api.Services;
using Carter;

namespace Krusty.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddCarter();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IJobStrategy, WeatherJob>();
            services.AddScoped<IJobFactory, JobFactory>();

            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IWeatherInfoService, WeatherInfoService>();

            services.AddWorkersPool();

            return services;
        }

        public static WebApplication UsePresentationServices(this WebApplication app)
        {
            app.MapCarter();

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

        private static IServiceCollection AddWorkersPool(this IServiceCollection services)
        {
            services.AddHostedService(s => new Worker(s.GetRequiredService<IServiceScopeFactory>(), JobProviders.WeatherConsuming));

            return services;
        }
    }
}
