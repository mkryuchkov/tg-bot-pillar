using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Storage.Configuration
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureStorageService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IStorageService, StorageService>();
            return services;
        }
    }
}