using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Bot.Input.Defaults;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class SaveQuestionHandler : IInputHandler
    {
        public string Name => "save_question";
        public async Task<string> Handle(IStorageService storageService,
            IDictionary<string, string> parameters, IDialogContext context,
            string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return HandlerResponse.Error;
            
            await storageService.SaveQuestion(
                context.ChatId,
                parameters[HandlerParameter.QuestionType],
                text);
            
            return HandlerResponse.Ok;
        }
    }
}