using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class ButtonExtensions
    {
        public static InlineKeyboardMarkup GetInlineKeyboard(
            this IEnumerable<Button> buttons) =>
            new(
                buttons.Select(button => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        button.Label, button.Transition)
                })
            );
    }
}