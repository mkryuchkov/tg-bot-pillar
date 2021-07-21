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

            var context = await _storageService.GetContextAsync(message.Chat.Id);
            var state = await _stateProcessor.GetStateAsync(context.State);

            if (context.State != DefaultState.Start || state.Input != null)
            {
                var newName = await _inputHandlersManager.HandleAsync(state.Input, context, message.Text);

                if (newName != context.State)
                {
                    await _storageService.UpdateStateAsync(message.Chat.Id, newName);
                    state = await _stateProcessor.GetStateAsync(newName);
                }
            }

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                state.Text,
                replyMarkup: state.GetKeyboard());
        }
    }
}