using System.Collections.Generic;

namespace TgBotPillar.StateProcessor.Model
{
    public class State
    {
        public string Text { get; init; }

        public IReadOnlyDictionary<string, Button> Buttons { get; init; }

        public Transition Transition { get; init; }
    }
}