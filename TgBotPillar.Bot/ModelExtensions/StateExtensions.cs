using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class StateExtensions
    {
        public static async Task<IReplyMarkup> GetKeyboard(this State state,
            IInputHandlersManager handlersManager,
            IDialogContext context,
            IStorageService storage)
        {
            return state.Input != null
                ? state.Input.GetKeyboard(handlersManager, context)
                : state.HasInlineMarkup()
                    ? await state.GetInlineKeyboard(handlersManager, context, storage)
                    : new ReplyKeyboardRemove();
        }

        public static async Task<InlineKeyboardMarkup> GetInlineKeyboard(this State state,
            IInputHandlersManager handlersManager,
            IDialogContext context,
            IStorageService storage)
        {
            return new InlineKeyboardMarkup(
                (state.Pager != null
                    ? await state.Pager.GetMarkup(context, storage)
                    : Enumerable.Empty<IEnumerable<InlineKeyboardButton>>())
                .Concat(state.Buttons.Count > 0
                    ? state.Buttons.GetInlineKeyboardButtons(handlersManager, context)
                    : Enumerable.Empty<IEnumerable<InlineKeyboardButton>>()
                ));
        }

        public static bool HasInlineMarkup(this State state)
        {
            return state.Buttons.Count > 0 || state.Pager != null;
        }

        public static async Task<string> GetFormattedText(this State state,
            IInputHandlersManager handlersManager,
            IDialogContext context,
            string messageText = null)
        {
            if (state.TextParameters.Count > 0)
            {
                return string.Format(state.Text,
                    await Task.WhenAll(state.TextParameters.Select(
                        handler => handlersManager.Handle(handler, context, messageText)))
                ).EscapeMarkdownV2();
            }
            return state.Text.EscapeMarkdownV2();
        }
    }
}