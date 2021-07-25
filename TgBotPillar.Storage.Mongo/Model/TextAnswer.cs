using TgBotPillar.Core.Model;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class TextAnswer : ITextAnswer
    {
        public long AuthorId { get; set; }
        
        public string Text { get; set; }
    }
}