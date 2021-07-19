using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TgBotPillar.Bot.Input.Configuration;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input
{
    public class InputHandlersManager : IInputHandlersManager
    {
        private readonly Task _initialization;
        private readonly ILogger<InputHandlersManager> _logger;
        private IReadOnlyDictionary<string, IInputHandler> _handlers;

        public InputHandlersManager(
            ILogger<InputHandlersManager> logger,
            IOptions<BotInputHandlersConfiguration> options)
        {
            _logger = logger;
            _initialization = InitializeAsync(options.Value);
        }

        private async Task InitializeAsync(BotInputHandlersConfiguration config)
        {
            _logger.LogInformation("Initialization started");

            _handlers = new[] {typeof(InputHandlersManager).Assembly.Location}.Concat(config.Assemblies)
                .SelectMany(assemblyName => Assembly.LoadFrom(assemblyName).GetTypes())
                .Where(type => typeof(IInputHandler).IsAssignableFrom(type)
                               && !type.IsInterface && !type.IsAbstract)
                .Select(type => (IInputHandler) Activator.CreateInstance(type))
                .ToDictionary(handler => handler?.Name, _ => _);

            _logger.LogInformation("Initialization completed");

            await Task.CompletedTask;
        }

        public async Task HandleAsync(string name, IDialogContext context)
        {
            await _initialization;
            _logger.LogInformation(message: await _handlers[name].HandleAsync(context) as string);
        }
    }
}