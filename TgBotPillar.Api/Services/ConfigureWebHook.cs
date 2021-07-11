using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
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

            InputFileStream certInput = null;
            try
            {
                var cert = File.OpenRead(_config.CertPath);
                certInput = new InputFileStream(cert);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Certificate is missing: ", ex.Message);
            }
            finally
            {
                await botClient.SetWebhookAsync(
                    webhookAddress,
                    certInput,
                    cancellationToken: cancellationToken);
                
                if (certInput?.Content != null) {
                    await certInput.Content.DisposeAsync();
                }
            }
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