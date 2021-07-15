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

            if (context.State == DefaultState.Start)
            {
                await (await _stateProcessor.GetStartStateAsync())
                    .SendNewMessageAsync(_botClient, message.Chat.Id);
            }
            // else
            // {
                // check state has input
                //   call input handler
                //     ok -> get new state
                //           create response (new state)
                //   bad -> create response (error message) // keep state
            // }
        }
    }
}