using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Telegram.Bot;
using TgBotPillar.Api.Model;
using TgBotPillar.Api.Services;

namespace TgBotPillar.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BotConfiguration>(
                Configuration.GetSection(nameof(BotConfiguration)));

            services.AddHostedService<ConfigureWebHook>();

            services.AddHttpClient("tgwebhook")
                    .AddTypedClient<ITelegramBotClient>(httpClient =>
                        new TelegramBotClient(
                            Configuration[$"{nameof(BotConfiguration)}:Token"],
                            httpClient));

            services.AddScoped<HandleUpdateService>();

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TgBotPillar.Api",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "TgBotPillar.Api v1"));
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "tgwebhook",
                    $"bot/{Configuration[$"{nameof(BotConfiguration)}:Token"]}",
                    new
                    {
                        controller = "webhook",
                        action = "Post"
                    });

                endpoints.MapControllers();
            });
        }
    }
}
