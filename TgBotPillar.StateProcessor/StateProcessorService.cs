using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.StateProcessor.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TgBotPillar.StateProcessor
{
    public class StateProcessorService : IStateProcessorService
    {
        private readonly ILogger<StateProcessorService> _logger;
        private readonly string _path;

        public StateProcessorService(
            ILogger<StateProcessorService> logger,
            IOptions<DialogProcessorConfiguration> options)
        {
            _logger = logger;
            _path = options.Value.FolderPath;
            // TODO: init from config folder ymls

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var dirInfo = new DirectoryInfo(_path);

            foreach (var fileInfo in dirInfo.GetFiles("*.yml"))
            {
                // WriteLine($"{fileInfo.Name}:"); // Filename
                var states = deserializer.Deserialize<
                    Dictionary<string, Dictionary<string, object>>>(
                    File.ReadAllText(fileInfo.FullName));
                foreach (var prop in states.SelectMany(state => state.Value))
                {
                    // Write($"    {prop.Key}: "); // State property (1 of 3)
                    switch (prop.Key)
                    {
                        case "text":
                            // WriteLine(prop.Value.ToString().Replace('\n', ' ')); // state.text value
                            break;
                        case "buttons":
                            if (prop.Value != null)
                            {
                                // WriteLine("");
                                foreach (var button in (List<object>) prop.Value)
                                {
                                    foreach (var buttonVal in (Dictionary<object, object>) button)
                                    {
                                        // WriteLine($"      - {buttonVal.Key}:"); // state.button name
                                        if (buttonVal.Value == null) continue;
                                        foreach (var buttonProp in (Dictionary<object, object>) buttonVal.Value)
                                        {
                                            // state.button properties
                                            // WriteLine($"          {buttonProp.Key}: {buttonProp.Value}");
                                        }
                                    }
                                }
                            }

                            break;
                        case "transition":
                            if (prop.Value is string transitionStr)
                            {
                                // WriteLine(transitionStr); // state.transition string value
                            }
                            else
                            {
                                // WriteLine("");
                                foreach (var transitionProp in (Dictionary<object, object>) prop.Value)
                                {
                                    // state.transition proprties (input, state)
                                    // WriteLine($"      {transitionProp.Key}: {transitionProp.Value}");
                                }
                            }

                            break;
                        default:
                            throw new Exception("Unexpected state property");
                    }
                }
                // WriteLine("");
            }
        }

        public Task<object> Process(object context, object update)
        {
            throw new NotImplementedException();
        }
    }
}