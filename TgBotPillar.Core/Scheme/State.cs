using System.Collections.Generic;

namespace TgBotPillar.Core.Scheme
{
    public class State
    {
        public string Text { get; init; }

        public IList<Handler> TextParameters { get; init; }
        
        public Pager Pager { get; init; }
        
        public IList<Button> Buttons { get; init; }

        public Input Input { get; init; }
        
        public string Transition { get; init; }
        
        public State()
        {
            TextParameters = new List<Handler>();
            Buttons = new List<Button>();
        }
    }
}