using System.Collections.Generic;
using MongoDB.Bson;
using TgBotPillar.Core.Model;
using MongoDB.Bson.Serialization.Attributes;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class DialogContext : IDialogContext
    {
        public ObjectId Id { get; set; }

        public long ChatId { get; set; }

        public string UserName { get; set; }

        [BsonDefaultValue(DefaultState.Start)]
        public string State { get; set; }

        public IList<string> Flags { get; set; }

        public DialogContext()
        {
            Flags = new List<string>();
        }
    }
}