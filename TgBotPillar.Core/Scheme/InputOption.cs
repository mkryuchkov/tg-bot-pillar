namespace TgBotPillar.Core.Scheme
{
    public class InputOption : IHasVisibilityHandler
    {
        public string Text { get; init; }
        
        public string Transition { get; init; }
        
        public Handler Visibility { get; init; }
    }
}