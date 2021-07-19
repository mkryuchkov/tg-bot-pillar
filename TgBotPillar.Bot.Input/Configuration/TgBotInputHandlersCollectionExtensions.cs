using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Bot.Input.Configuration
{
    public static class TgBotInputHandlersCollectionExtensions
    {
        public static IServiceCollection ConfigureTgBotInputHandlers(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BotInputHandlersConfiguration>(
                configuration.GetSection(nameof(BotInputHandlersConfiguration)));

            services.AddSingleton<IInputHandlersManager, InputHandlersManager>();

            return services;
        }
    }
}