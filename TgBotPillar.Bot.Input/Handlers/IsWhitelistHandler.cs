using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class IsWhitelistHandler : IInputHandler
    {
        public string Name => "is_whitelist";

        public Task<string> Handle(IStorageService storageService,
            IDictionary<string, string> parameters, IDialogContext context,
            string text)
        {
            return Task.FromResult(context.IsWhitelisted.ToString());
        }
    }
}