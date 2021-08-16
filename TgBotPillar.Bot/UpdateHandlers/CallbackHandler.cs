using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPillar.Bot.ModelExtensions;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            _logger.LogInformation($"Receive CallbackQuery {callbackQuery.Id}: {callbackQuery.Data}");

            var oldStateName = (await _storageService.GetContext(
                    callbackQuery.Message.Chat.Id, callbackQuery.From.Username))
                .State;

            await _storageService.UpdateState(
                callbackQuery.Message.Chat.Id, callbackQuery.From.Username,
                callbackQuery.Data);

            var newState = await _stateProcessor.GetState(callbackQuery.Data);
            var newContext = await _storageService.GetContext(
                callbackQuery.Message.Chat.Id, callbackQuery.From.Username);

            if (newState.Buttons.Count > 0)
            {
                await _botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId,
                    await newState.GetFormattedText(_inputHandlersManager, newContext),
                    replyMarkup: newState.Buttons.GetInlineKeyboard(_inputHandlersManager, newContext));
            }
            else
            {
                var oldState = await _stateProcessor.GetState(oldStateName);
                await _botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId,
                    $"`>>>` _{oldState.Buttons.First(b => b.Transition == callbackQuery.Data).Label}_",
                    ParseMode.MarkdownV2,
                    replyMarkup: null);

                await _botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    await newState.GetFormattedText(_inputHandlersManager, newContext),
                    replyMarkup: newState.GetKeyboard(_inputHandlersManager, newContext));
            }
        }
    }
}