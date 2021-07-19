using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Storage.InMemory
{
    public class StorageService : IStorageService
    {
        private readonly ILogger<StorageService> _logger;
        private readonly ConcurrentDictionary<long, string> _contextStorage;

        public StorageService(ILogger<StorageService> logger)
        {
            _logger = logger;
            _contextStorage = new ConcurrentDictionary<long, string>();
        }

        public Task<DialogContext> GetContextAsync(long chatId)
        {
            _logger.LogInformation($"Get context for {chatId} chat");
            return Task.FromResult(new DialogContext
            {
                State = _contextStorage.GetOrAdd(chatId, _ => DefaultState.Start)
            });
        }

        public Task UpdateStateAsync(long chatId, string stateName)
        {
            _contextStorage.AddOrUpdate(chatId,
                _ => stateName,
                (_, _) => stateName);
            _logger.LogInformation($"Update state for {chatId} to {stateName}");
            return Task.CompletedTask;
        }
    }
}