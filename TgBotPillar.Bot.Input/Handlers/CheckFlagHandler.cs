using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Bot.Input.Defaults;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class CheckFlagHandler : IInputHandler
    {
        public string Name => "check_flag";

        public Task<string> Handle(IStorageService storageService,
            IDictionary<string, string> parameters, IDialogContext context,
            string text)
        {
            return Task.FromResult(
                context.Flags.Contains(parameters[HandlerParameter.FlagName])
                    .ToString());
        }
    }
}