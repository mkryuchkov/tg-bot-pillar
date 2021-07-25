using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class TextQuestion : IQuestion
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        public long ChatId { get; set; }

        public string Text { get; set; }
        
        public IList<ITextAnswer> Answers { get; set; }

        public TextQuestion()
        {
            Answers = new List<ITextAnswer>();
        }
    }
}