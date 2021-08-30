using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class GetQuestionHandler : IInputHandler
    {
        public string Name => "get_question";

        public async Task<string> Handle(IStorageService storageService,
            IDictionary<string, string> parameters, IDialogContext context, string text)
        {
            return (await storageService.GetQuestion(context.ChatId,
                    await storageService.UnStash<string>(
                        context.ChatId, $"{context.State}:question:type"),
                    await storageService.UnStash<string>(
                        context.ChatId, $"{context.State}:question:id")))
                .ToString();
        }
    }
}