using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TgBotPillar.Api.Model;

namespace TgBotPillar.Api.Services
{
    internal class ConfigureWebHook : IHostedService
    {
        private readonly ILogger<ConfigureWebHook> _logger;
        private readonly IServiceProvider _services;
        private readonly BotConfiguration _config;

        public ConfigureWebHook(
            ILogger<ConfigureWebHook> logger,
            IServiceProvider serviceProvider,
            IOptions<BotConfiguration> options)
        {
            _logger = logger;
            _services = serviceProvider;
            _config = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Configure custom endpoint per Telegram API recommendations:
            // https://core.telegram.org/bots/api#setwebhook
            // If you'd like to make sure that the Webhook request comes from Telegram, we recommend
            // using a secret path in the URL, e.g. https://www.example.com/<token>.
            // Since nobody else knows your bot's token, you can be pretty sure it's us.
            var webhookAddress = @$"{_config.Host}/bot/{_config.Token}";
            _logger.LogInformation("Setting webhook: ", webhookAddress);
            await botClient.SetWebhookAsync(webhookAddress, cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            // Remove webhook upon app shutdown
            _logger.LogInformation("Removing webhook");
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}