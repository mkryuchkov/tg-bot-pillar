using System.Collections.Generic;

namespace TgBotPillar.Bot.Input.Configuration
{
    public class BotInputHandlersConfiguration
    {
        public IList<string> Assemblies { get; init; }

        public IDictionary<string, IList<string>> UserFlags { get; init; }
        
        public BotInputHandlersConfiguration()
        {
            Assemblies = new List<string>();
            UserFlags = new Dictionary<string, IList<string>>();
        }
    }
}