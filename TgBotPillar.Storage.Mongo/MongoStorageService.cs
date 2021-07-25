using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
            return await _contextCollection
                       .Find(_ => _.ChatId == chatId)
                       .FirstOrDefaultAsync()
                   ?? new DialogContext {ChatId = chatId, State = DefaultState.Start};
        }

        public async Task UpdateState(long chatId, string stateName)
        {
            _logger.LogInformation($"Updating state for {chatId} to {stateName}");
            await _contextCollection.UpdateOneAsync(
                _ => _.ChatId == chatId,
                Builders<DialogContext>.Update.Set(_ => _.State, stateName),
                new UpdateOptions
                {
                    IsUpsert = true
                });
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

        public async Task<IQuestion> GetQuestion(long chatId, string questionType)
        {
            _logger.LogInformation($"Getting {questionType} question for {chatId}");
            return await _db
                .GetCollection<TextQuestion>(questionType)
                .AsQueryable()
                .Sample(1)
                .FirstOrDefaultAsync();
        }

        public Task Stash<TValue>(long chatId, string key, TValue value)
        {
            _logger.LogInformation($"Saving {chatId}.Stash[{key}]={value}");
            return _db
                .GetCollection<Stash>(nameof(Stash))
                .UpdateOneAsync(
                    _ => _.Key == $"{chatId}:{key}",
                    Builders<Stash>.Update.Set(_ => _.Value, value),
                    new UpdateOptions
                    {
                        IsUpsert = true
                    });
        }

        public async Task<TValue> UnStash<TValue>(long chatId, string key)
        {
            _logger.LogInformation($"Getting {chatId}.Stash[{key}]");
            return (TValue)(await _db
                    .GetCollection<Stash>(nameof(Stash))
                    .Find(_ => _.Key == $"{chatId}:{key}")
                    .FirstOrDefaultAsync())
                .Value;
        }

        public Task SaveAnswer(long chatId, string questionType, string questionId, string text)
        {
            _logger.LogInformation($"Saving answer for {questionType} question text:\n{text}");
            return _db
                .GetCollection<TextQuestion>(questionType)
                .FindOneAndUpdateAsync(
                    _ => _.Id == questionId,
                    Builders<TextQuestion>.Update.Push(
                        _ => _.Answers,
                        new TextAnswer
                        {
                            AuthorId = chatId,
                            Text = text
                        }));
        }
    }
}