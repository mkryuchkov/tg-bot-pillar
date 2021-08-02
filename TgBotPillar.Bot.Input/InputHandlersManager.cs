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
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.Input
{
    public class InputHandlersManager : IInputHandlersManager
    {
        private readonly Task _initialization;
        private readonly ILogger<InputHandlersManager> _logger;
        private readonly IStorageService _storageService;
        private IReadOnlyDictionary<string, IInputHandler> _handlers;

        public InputHandlersManager(
            ILogger<InputHandlersManager> logger,
            IOptions<BotInputHandlersConfiguration> options,
            IStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
            _initialization = Initialize(options.Value);
        }

        private async Task Initialize(BotInputHandlersConfiguration config)
        {
            _logger.LogInformation("Initialization started");

            _handlers = new[] {typeof(InputHandlersManager).Assembly.Location}
                .Concat(config.Assemblies)
                .SelectMany(assemblyName => Assembly.LoadFrom(assemblyName).GetTypes())
                .Where(type => typeof(IInputHandler).IsAssignableFrom(type)
                               && !type.IsInterface && !type.IsAbstract)
                .Select(type => (IInputHandler) Activator.CreateInstance(type))
                .ToDictionary(handler => handler?.Name, _ => _);

            await Task.WhenAll(config.UserFlags
                .Where(_ => _.Value.Count > 0)
                .Select(_ => _storageService.SetUserFlags(_.Key, _.Value)));

            _logger.LogInformation("Initialization completed");
        }

        public async Task<string> Handle(Core.Scheme.Input input, IDialogContext context, string text = null)
        {
            await _initialization;

            string newState = null;

            if (input.Options.Count > 0 && !string.IsNullOrEmpty(text))
            {
                newState = input.Options.FirstOrDefault(option => option.Text == text)?.Transition;
            }

            if (input.Handler != null)
            {
                newState = await Handle(input.Handler, context, text) ?? newState;
            }

            return string.IsNullOrEmpty(newState)
                ? input.DefaultTransition
                : newState.ToLowerInvariant();
        }

        public async Task<string> Handle(Handler handler, IDialogContext context, string text = null)
        {
            await _initialization;

            if (!_handlers.ContainsKey(handler.Name))
                throw new ArgumentException($"Handler `{handler.Name}` not found.");

            var handlerResult = await _handlers[handler.Name]
                .Handle(_storageService, handler.Parameters, context, text);

            return handler.Switch.TryGetValue(handlerResult.ToLowerInvariant(), out var switchState)
                ? switchState
                : handlerResult;
        }
    }
}