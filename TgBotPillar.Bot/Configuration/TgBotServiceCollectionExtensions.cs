using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Bot.Configuration
{
    public static class TgBotServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureTgBot(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BotConfiguration>(
                configuration.GetSection(nameof(BotConfiguration)));

            services.AddHostedService<ConfigureWebHook>();

            services.AddHttpClient("tgWebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient =>
                    new TelegramBotClient(
                        configuration[$"{nameof(BotConfiguration)}:Token"],
                        httpClient));

            services.AddScoped<IUpdateHandlerService<Update>, UpdateHandlerService>();

            services
                .AddControllers()
                .AddNewtonsoftJson();

            return services;
        }
    }
}