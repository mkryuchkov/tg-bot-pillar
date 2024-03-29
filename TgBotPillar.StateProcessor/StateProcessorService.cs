using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Scheme;
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
            Initialization = Initialize(options.Value.FolderPath);
        }

        public Task Initialization { get; }

        public IReadOnlyDictionary<string, State> States { get; private set; }

        private async Task Initialize(string folderPath)
        {
            _logger.LogInformation("Initialization started");
            var allStates = new Dictionary<string, State>();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var dirInfo = new DirectoryInfo(folderPath);

            foreach (var fileInfo in dirInfo.GetFiles("*.yml"))
            {
                var states = deserializer.Deserialize<Dictionary<string, State>>(
                                 await File.ReadAllTextAsync(fileInfo.FullName))
                             ?? new Dictionary<string, State>();

                foreach (var (key, value) in states)
                {
                    allStates.Add(key, value);
                }
            }

            States = allStates;
            _logger.LogInformation("Initialization completed");
        }

        public async Task<State> GetState(string stateName)
        {
            await Initialization;
            _logger.LogInformation($"Get {stateName} state");
            return States.TryGetValue(stateName, out var state)
                ? state
                : States[DefaultState.Start];
        }

        public async Task<Tuple<string, State>> GetNewStateAsync(string state, string inputText)
        {
            await Initialization;
            _logger.LogInformation($"Get new state by input text {inputText}");
            var stateName = States[state].Input.Options
                                .FirstOrDefault(option => option.Text == inputText)?.Transition
                            ?? States[state].Input.DefaultTransition
                            ?? state;
            return new Tuple<string, State>(stateName, States[stateName]);
        }
    }
}