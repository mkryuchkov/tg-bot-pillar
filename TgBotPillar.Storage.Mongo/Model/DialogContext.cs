using System.Collections.Generic;
using TgBotPillar.Core.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace TgBotPillar.Storage.Mongo.Model
{
    public class DialogContext : IDialogContext
    {
        [BsonId] public long ChatId { get; set; }

        public string UserName { get; set; }

        public string State { get; set; }

        public IList<string> Flags { get; set; }

        public DialogContext()
        {
            Flags = new List<string>();
        }
    }
}