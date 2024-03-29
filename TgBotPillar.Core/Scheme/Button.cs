using TgBotPillar.Common;

namespace TgBotPillar.Core.Scheme
{
    public class Button : IHasVisibilityHandler
    {
        private readonly string _label;

        public string Label
        {
            get => _label ?? Transition.FirstCharToUpper();
            init => _label = value;
        }

        public string Transition { get; init; }
        
        public Handler Visibility { get; init; }
    }
}