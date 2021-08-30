using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPillar.Bot.ModelExtensions;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnMessageReceived(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            await ProcessMessage(message);
        }

        private async Task ProcessMessage(Message message)
        {
            var context = await _storageService.GetContext(
                message.Chat.Id, message.From.Username);
            var state = await _stateProcessor.GetState(context.State);

            if (state.Input != null)
            {
                var newName = await _inputHandlersManager.Handle(state.Input, context, message.Text);

                if (newName != context.State)
                {
                    await _storageService.UpdateState(message.Chat.Id, message.From.Username, newName);
                    state = await _stateProcessor.GetState(newName);
                }
            }

            if (string.IsNullOrWhiteSpace(state.Text))
                return;

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                await state.GetFormattedText(_inputHandlersManager, context, message.Text),
                ParseMode.MarkdownV2,
                replyMarkup: await state.GetKeyboard(_inputHandlersManager, context, _storageService));

            if (!string.IsNullOrWhiteSpace(state.Transition))
            {
                await _storageService.UpdateState(message.Chat.Id, message.From.Username, state.Transition);
                await ProcessMessage(message);
            }
        }
    }
}