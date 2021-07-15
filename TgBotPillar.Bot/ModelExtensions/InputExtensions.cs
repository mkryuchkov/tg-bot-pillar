using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class InputExtensions
    {
        public static ReplyKeyboardMarkup GetKeyboard(this Input input)
        {
            return new(
                new[]
                {
                    input.Options.Select(option => new KeyboardButton(option.Text))
                },
                resizeKeyboard: true,
                oneTimeKeyboard: true
            );
        }
    }
}