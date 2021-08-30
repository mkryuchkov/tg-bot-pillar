using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Common;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class PagerExtensions
    {
        private const int PageSize = 5;
        private const int MaxButtonLength = 15;

        public static async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetMarkup(
            this Pager pager,
            IDialogContext context,
            IStorageService storage)
        {
            var pageNumber = await storage.UnStash<int>(
                context.ChatId, $"{context.State}:pager:{pager.QuestionType}");

            var (questions, questionsCount) =
                await storage.GetQuestions(context.ChatId, pager.QuestionType, PageSize, pageNumber);

            var result = questions
                .Select(_ =>
                {
                    return new[]
                    {
                        InlineKeyboardButton.WithCallbackData(
                            $"\"{_.Text.Truncate(MaxButtonLength)}...\"",
                            $"pager:entry:{pager.QuestionType}:{_.Id}")
                    };
                })
                .Append(new[]
                {
                    pageNumber > 0
                        ? InlineKeyboardButton.WithCallbackData(
                            $"({pageNumber}) <<",
                            $"pager:page:{pageNumber - 1}")
                        : null,
                    pageNumber < (questionsCount - 1) / PageSize
                        ? InlineKeyboardButton.WithCallbackData(
                            $">> ({pageNumber + 2})",
                            $"pager:page:{pageNumber + 1}")
                        : null
                }.Where(_ => _ != null));

            return result;
        }
    }
}