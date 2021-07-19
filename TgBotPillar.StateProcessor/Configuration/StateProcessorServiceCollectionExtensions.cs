using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.StateProcessor.Configuration
{
    public static class StateProcessorServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureStateProcessor(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<StateProcessorConfiguration>(
                configuration.GetSection(nameof(StateProcessorConfiguration)));

            services.AddSingleton<IStateProcessorService, StateProcessorService>();

            return services;
        }
    }
}