using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.Storage.Mongo.Configuration;
using TgBotPillar.Storage.Mongo.Model;

namespace TgBotPillar.Storage.Mongo
{
    public class MongoStorageService : IStorageService
    {
        private readonly ILogger<MongoStorageService> _logger;
        private readonly IMongoCollection<DialogContext> _contextCollection;
        private readonly IMongoDatabase _db;

        public MongoStorageService(
            ILogger<MongoStorageService> logger,
            IOptions<MongoStorageSettings> options)
        {
            _logger = logger;

            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.DatabaseName);
            _contextCollection = _db.GetCollection<DialogContext>(
                nameof(DialogContext));
        }

        public async Task<IDialogContext> GetContext(long chatId)
        {
            _logger.LogInformation($"Getting context for {chatId} chat");
            
            var result = await _contextCollection
                             .Find(_ => _.ChatId == chatId)
                             .FirstOrDefaultAsync();
            
            return result ?? new DialogContext {ChatId = chatId, State = DefaultState.Start};
        }

        public async Task UpdateState(long chatId, string stateName)
        {
            _logger.LogInformation($"Updating state for {chatId} to {stateName}");
            await _contextCollection.ReplaceOneAsync(_ => _.ChatId == chatId,
                new DialogContext {ChatId = chatId, State = stateName},
                new ReplaceOptions {IsUpsert = true});
        }

        public Task SaveQuestion(long chatId, string questionType, string text)
        {
            _logger.LogInformation($"Saving {questionType} question text:\n{text}\nfor {chatId}");
            return _db
                .GetCollection<TextQuestion>(questionType)
                .InsertOneAsync(new TextQuestion
                {
                    ChatId = chatId,
                    Text = text
                });
        }
    }
}