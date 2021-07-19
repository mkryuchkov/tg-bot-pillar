using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TgBotPillar.Bot.Configuration;
using TgBotPillar.Bot.Input.Configuration;
using TgBotPillar.StateProcessor.Configuration;
using TgBotPillar.Storage.InMemory.Configuration;

namespace TgBotPillar.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // todo: choose storage depending on config
            services.ConfigureInMemoryStorageService();
            services.ConfigureStateProcessor(Configuration);
            services.ConfigureTgBotInputHandlers(Configuration);
            services.ConfigureTgBot(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "tgWebhook",
                    $"bot/{Configuration[$"{nameof(BotConfiguration)}:Token"]}",
                    new
                    {
                        controller = "tgWebhook",
                        action = "Post"
                    });
                // endpoints.MapControllers();
            });
        }
    }
}