using MongoDB.Bson.Serialization.Attributes;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class TextQuestion
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
    }
}