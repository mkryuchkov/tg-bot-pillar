using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Bot.Input.Defaults;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class GetQuestionHandler : IInputHandler
    {
        public string Name => "get_question";

        public async Task<string> Handle(
            IStorageService storageService,
            IDictionary<string, string> parameters,
            IDialogContext context,
            string text)
        {
            var result = await storageService.GetQuestion(
                context.ChatId,
                parameters[HandlerParameter.QuestionType]);

            await storageService.Stash(
                context.ChatId,
                HandlerStashKey.QuestionId,
                result.Id);

            return result.Text;
        }
    }
}