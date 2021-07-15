using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TgBotPillar.Bot.ModelExtensions;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnCallbackQueryReceivedAsync(CallbackQuery callbackQuery)
        {
            _logger.LogInformation($"Receive CallbackQuery {callbackQuery.Id}: {callbackQuery.Data}");

            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id,
                $"Received {callbackQuery.Data}");
            
            var state = await _stateProcessor.GetStateAsync(callbackQuery.Data);

            // todo: what if new state have input!
            
            await _storageService.UpdateStateAsync(callbackQuery.Message.Chat.Id, callbackQuery.Data);
            
            await state.UpdateLastMessageAsync(_botClient,
                callbackQuery.Message.Chat.Id,
                callbackQuery.Message.MessageId);
        }
    }
}