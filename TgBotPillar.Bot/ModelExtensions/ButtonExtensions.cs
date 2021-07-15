using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class ButtonExtensions
    {
        public static InlineKeyboardMarkup GetKeyboardMarkup(
            this IReadOnlyDictionary<string, Button> buttons)
        {
            return new(new[]
                {
                    buttons.Select(button =>
                        InlineKeyboardButton.WithCallbackData(
                            button.Value.Label, button.Key)
                    )
                }
            );
        }
    }
}