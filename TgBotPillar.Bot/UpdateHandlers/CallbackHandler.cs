using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TgBotPillar.Bot.ModelExtensions;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            _logger.LogInformation($"Receive CallbackQuery {callbackQuery.Id}: {callbackQuery.Data}");
            
            await _storageService.UpdateState(callbackQuery.Message.Chat.Id, callbackQuery.Data);

            var state = await _stateProcessor.GetState(callbackQuery.Data);

            if (state.Input != null)
            {
                await _botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    state.Text,
                    replyMarkup: state.Input.GetKeyboard());
            }
            else
            {
                await _botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId,
                    state.Text,
                    replyMarkup: state.Buttons.GetInlineKeyboard());
            }
        }
    }
}