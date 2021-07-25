using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class StateExtensions
    {
        public static IReplyMarkup GetKeyboard(this State state)
        {
            return state.Input != null
                ? state.Input.GetKeyboard()
                : state.Buttons.GetInlineKeyboard();
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
                );
            }

            return state.Text;
        }
    }
}