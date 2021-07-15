using TgBotPillar.Common;

namespace TgBotPillar.Core.Model
{
    public class Button
    {
        private readonly string _label;

        public string Label
        {
            get => _label ?? Transition.FirstCharToUpper();
            init => _label = value;
        }

        public string Transition { get; init; }
    }
}