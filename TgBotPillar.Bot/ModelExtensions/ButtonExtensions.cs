using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class ButtonExtensions
    {
        public static InlineKeyboardMarkup GetInlineKeyboard(this IEnumerable<Button> buttons,
            IInputHandlersManager handlersManager, IDialogContext context) =>
            new(buttons.GetInlineKeyboardButtons(handlersManager, context));

        public static IEnumerable<IEnumerable<InlineKeyboardButton>> GetInlineKeyboardButtons(
            this IEnumerable<Button> buttons,
            IInputHandlersManager handlersManager, IDialogContext context) =>
            buttons
                .FilterVisible(handlersManager, context)
                .Select(button => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        button.Label, button.Transition)
                });
    }
}