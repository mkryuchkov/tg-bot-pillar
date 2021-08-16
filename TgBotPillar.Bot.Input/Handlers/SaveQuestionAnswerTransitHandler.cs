using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Bot.Input.Defaults;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class SaveQuestionAnswerTransitHandler : IInputHandler
    {
        public string Name => "save_question_answer_transit";

        public async Task<string> Handle(IStorageService storageService,
            IDictionary<string, string> parameters, IDialogContext context,
            string text)
        {
            var questionId = await storageService
                .UnStash<string>(context.ChatId, HandlerStashKey.QuestionId);

            await storageService.SaveAnswer(
                context.ChatId,
                parameters[HandlerParameter.QuestionType],
                questionId,
                text);

            return text;
        }
    }
}