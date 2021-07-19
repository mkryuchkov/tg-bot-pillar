using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Storage.Mongo.Configuration
{
    public static class MongoStorageServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureMongoStorage(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoStorageSettings>(
                configuration.GetSection(nameof(MongoStorageSettings)));
            services.AddSingleton<IStorageService, MongoStorageService>();
            return services;
        }
    }
}