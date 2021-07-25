﻿using System;
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

            _logger.LogInformation("Initialization completed");

            await Task.CompletedTask;
        }

        public async Task<string> Handle(Core.Model.Input input, IDialogContext context, string text = null)
        {
            await _initialization;

            string newState = null;

            if (input.Options.Count > 0 && !string.IsNullOrEmpty(text))
            {
                newState = input.Options.FirstOrDefault(option => option.Text == text)?.Transition;
            }

            if (input.Handler != null)
            {
                if (!_handlers.ContainsKey(input.Handler.Name))
                    throw new ArgumentException($"Handler `{input.Handler.Name}` not found.");

                var handlerResult = (await _handlers[input.Handler.Name]
                        .Handle(_storageService, input.Handler.Parameters, context, text))
                    .ToLowerInvariant();
                newState = input.Handler.Switch.TryGetValue(handlerResult, out var switchState)
                    ? switchState
                    : newState;
            }

            return string.IsNullOrEmpty(newState)
                ? input.DefaultTransition
                : newState;
        }
    }
}