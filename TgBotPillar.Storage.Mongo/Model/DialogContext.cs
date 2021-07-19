using TgBotPillar.Core.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class DialogContext : IDialogContext
    {
        [BsonId] public long ChatId { get; set; }

        public string State { get; set; }
    }
}