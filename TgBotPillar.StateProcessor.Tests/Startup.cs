using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotPillar.StateProcessor.Configuration;

namespace TgBotPillar.StateProcessor.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.ConfigureStateProcessor(context.Configuration);
        }

        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
                    {
                        new("StateProcessorConfiguration:FolderPath", "./States")
                    });
                });
    }
}