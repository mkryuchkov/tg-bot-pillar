using System.Collections.Generic;

namespace TgBotPillar.Core.Model
{
    public interface IQuestion
    {
        public string Id { get; }
        
        public long ChatId { get; }
        
        public string Text { get; }
        
        IList<ITextAnswer> Answers { get; }
    }
}