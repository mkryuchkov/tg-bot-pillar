using System.Collections.Generic;

namespace TgBotPillar.Core.Model
{
    public class Input
    {
        public string Handler { get; init; }
        
        public IList<InputOption> Options { get; init; }
        
        public string DefaultTransition { get; init; }
        
        public Input()
        {
            Options = new List<InputOption>();
        }
    }
}