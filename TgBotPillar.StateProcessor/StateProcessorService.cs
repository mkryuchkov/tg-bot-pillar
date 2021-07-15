using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TgBotPillar.Common;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.StateProcessor.Configuration;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TgBotPillar.StateProcessor
{
    public class StateProcessorService : IStateProcessorService
    {
        private readonly ILogger<StateProcessorService> _logger;

        public StateProcessorService(
            ILogger<StateProcessorService> logger,
            IOptions<StateProcessorConfiguration> options)
        {
            _logger = logger;

            States = new Dictionary<string, State>();

            Initialization = InitializeAsync(options.Value.FolderPath);
        }

        public Task Initialization { get; }

        public IReadOnlyDictionary<string, State> States { get; private set; }

        private async Task InitializeAsync(string folderPath)
        {
            _logger.LogInformation("Initialisation started");
            var allStates = new Dictionary<string, State>();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var dirInfo = new DirectoryInfo(folderPath);

            foreach (var fileInfo in dirInfo.GetFiles("*.yml"))
            {
                var states = deserializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(
                    await File.ReadAllTextAsync(fileInfo.FullName));

                foreach (var (key, value) in states.AsEnumerable())
                {
                    var buttons = value.TryGetValue("buttons", out var buttonValues)
                        ? ((Dictionary<object, object>) buttonValues)
                        .Select(button => new KeyValuePair<string, Button>((string) button.Key, new Button
                        {
                            Label = ((string) (button.Value is Dictionary<object, object> buttonDict
                                ? buttonDict.TryGetValue("label", out var label) ? label : button.Key
                                : button.Key)).FirstCharToUpper(),
                            NextState = (string) (button.Value is Dictionary<object, object> buttonDict2
                                ? buttonDict2.TryGetValue("transition", out var buttonTransition)
                                    ? buttonTransition
                                    : button.Key
                                : button.Key)
                        }))
                        .ToDictionary(_ => _.Key, _ => _.Value)
                        : new Dictionary<string, Button>();

                    allStates.Add(key, new State
                    {
                        Text = (value.TryGetValue("text", out var text) ? text : string.Empty) as string,
                        Buttons = buttons,
                        Transition =
                            (value.TryGetValue("transition", out var transition) ? transition : string.Empty) as string,
                        Input = (value.TryGetValue("input", out var input) ? input : string.Empty) as string,
                    });
                }
            }

            States = allStates;
            _logger.LogInformation("Initialisation finished");
        }

        public async Task<State> GetStartStateAsync()
        {
            await Initialization;
            _logger.LogInformation("Get start state");
            return States["start"];
        }

        public async Task<Tuple<string, State>> GetNextStateAsync(string state, string button)
        {
            await Initialization;
            var nextState = States[state].Buttons[button].NextState;
            _logger.LogInformation($"Get next state for {state}[{button}]: {States[nextState]}");
            return new Tuple<string, State>(nextState, States[nextState]);
        }
    }
}