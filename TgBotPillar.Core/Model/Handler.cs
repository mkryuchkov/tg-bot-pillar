using System.Collections.Generic;

namespace TgBotPillar.Core.Model
{
    public class Handler
    {
        public string Name { get; init; }
        
        public IDictionary<string, string> Parameters { get; init; }
        
        public IDictionary<string, string> Switch { get; init; }

        public Handler()
        {
            Parameters = new Dictionary<string, string>();
            Switch = new Dictionary<string, string>();
        }
    }
}