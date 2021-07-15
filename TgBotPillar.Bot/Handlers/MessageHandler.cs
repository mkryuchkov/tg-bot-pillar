using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPillar.Bot.ModelExtensions;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnMessageReceivedAsync(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");

            if (message.Type != MessageType.Text)
                return;

            var stateName = (await _storageService.GetContextAsync(message.Chat.Id)).State;
            var state = await _stateProcessor.GetStateAsync(stateName);

            if (stateName != DefaultState.Start || state.Input != null)
            {
                var (newName, newState) = await _stateProcessor.GetNewStateAsync(stateName, message.Text);
                if (newName != stateName)
                {
                    await _storageService.UpdateStateAsync(message.Chat.Id, newName);
                    state = newState;
                }
            }

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                state.Text,
                replyMarkup: state.GetKeyboard());
        }
    }
}