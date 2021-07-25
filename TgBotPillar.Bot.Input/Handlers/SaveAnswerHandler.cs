using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Bot.Input.Defaults;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class SaveAnswerHandler : IInputHandler
    {
        public string Name => "save_answer";

        public async Task<string> Handle(IStorageService storageService, IDictionary<string, string> parameters,
            IDialogContext context, string text)
        {
            await storageService.SaveQuestion(
                context.ChatId,
                parameters[HandlerParameter.QuestionType],
                text);

            return text;
        }
    }
}