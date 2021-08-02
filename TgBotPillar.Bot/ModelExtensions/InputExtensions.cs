using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class InputExtensions
    {
        public static ReplyKeyboardMarkup GetKeyboard(this Input input,
            IInputHandlersManager handlersManager, IDialogContext context) =>
            new(
                input.Options
                    .FilterVisible(handlersManager, context)
                    .Select(option => new[]
                    {
                        new KeyboardButton(option.Text)
                    }),
                true,
                true
            );
    }
}