using System.Collections.Generic;

namespace TgBotPillar.Core.Scheme
{
    public class Input
    {
        public Handler Handler { get; init; }
        
        public IList<InputOption> Options { get; init; }
        
        public string DefaultTransition { get; init; }
        
        public Input()
        {
            Options = new List<InputOption>();
        }
    }
}