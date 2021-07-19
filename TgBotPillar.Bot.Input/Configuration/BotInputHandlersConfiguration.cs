using System.Collections.Generic;

namespace TgBotPillar.Bot.Input.Configuration
{
    public class BotInputHandlersConfiguration
    {
        public IList<string> Assemblies { get; init; }

        public BotInputHandlersConfiguration()
        {
            Assemblies = new List<string>();
        }
    }
}