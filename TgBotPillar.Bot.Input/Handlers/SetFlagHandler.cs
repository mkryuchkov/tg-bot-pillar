using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgBotPillar.Bot.Input.Defaults;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class SetFlagHandler : IInputHandler
    {
        public string Name => "set_flag";

        public async Task<string> Handle(IStorageService storageService,
            IDictionary<string, string> parameters, IDialogContext context,
            string text)
        {
            var userName = text[1..].Split().FirstOrDefault();
            
            if (string.IsNullOrEmpty(userName) || text[0] != '@')
                return false.ToString();
            
            await storageService.SetUserFlag(userName, parameters[HandlerParameter.FlagName]);
            return true.ToString();
        }
    }
}