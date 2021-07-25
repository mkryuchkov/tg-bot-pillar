namespace TgBotPillar.Core.Model
{
    public interface ITextAnswer
    {
        long AuthorId { get; }
        
        string Text { get; }
    }
}