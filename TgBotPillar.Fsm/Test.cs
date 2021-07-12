using System.Collections.Generic;
using System.IO;
using TbBotPillar.Fsm.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TbBotPillar.Fsm
{
    public class Test
    {
        private readonly IDeserializer deserializer;

        public Test()
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }

        public void X()
        {
            var dirInfo = new DirectoryInfo("States");

            foreach (var fileInfo in dirInfo.GetFiles("*.yml"))
            {
                var states = deserializer.Deserialize<
                    Dictionary<string, Dictionary<string, object>>>(
                        File.ReadAllText(fileInfo.FullName));
                foreach (var state in states)
                {
                    // WriteLine($"  - {state.Key}");
                    foreach (var prop in state.Value)
                    {
                        // Write($"    - {prop.Key}: ");
                        switch (prop.Key)
                        {
                            case "text":
                                // WriteLine(prop.Value);
                                break;
                            case "buttons":
                                foreach (var button in (IEnumerable<object>)prop.Value) {
                                    // WriteLine($"      - {button.Key}: {button.Value}");
                                }
                                break;
                            case "transition":
                                // WriteLine(prop.Value);
                                break;
                            default:
                                throw new System.Exception("Unexpected state property");
                        }
                    }
                }
            }
        }
    }
}