using Telegram.Bot.Types.ReplyMarkups;
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
    }
}