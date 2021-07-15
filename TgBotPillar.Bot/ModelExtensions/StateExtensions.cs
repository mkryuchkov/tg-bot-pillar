using System.Threading.Tasks;
using Telegram.Bot;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class StateExtensions
    {
        public static async Task UpdateLastMessageAsync(this State state,
            ITelegramBotClient bot,
            long chatId, int messageId)
        {
            await bot.EditMessageTextAsync(chatId, messageId,
                state.Text,
                replyMarkup: state.Buttons.GetKeyboardMarkup());
        }
        
        public static async Task SendNewMessageAsync(this State state,
            ITelegramBotClient bot,
            long chatId)
        {
            await bot.SendTextMessageAsync(chatId,
                state.Text,
                replyMarkup: state.Buttons.GetKeyboardMarkup());
        }
    }
}