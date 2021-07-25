namespace TgBotPillar.Core.Model
{
    public interface IDialogContext
    {
        public long ChatId { get; }
        
        string State { get; }
        
        bool IsWhitelisted { get; }
    }
}