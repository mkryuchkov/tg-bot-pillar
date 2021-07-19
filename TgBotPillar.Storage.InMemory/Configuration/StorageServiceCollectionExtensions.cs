using Microsoft.Extensions.DependencyInjection;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Storage.InMemory.Configuration
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureInMemoryStorageService(
            this IServiceCollection services)
        {
            services.AddSingleton<IStorageService, StorageService>();
            return services;
        }
    }
}