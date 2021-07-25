using MongoDB.Bson.Serialization.Attributes;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class Stash
    {
        [BsonId] public string Key { get; set; }

        public object Value { get; set; }
    }
}