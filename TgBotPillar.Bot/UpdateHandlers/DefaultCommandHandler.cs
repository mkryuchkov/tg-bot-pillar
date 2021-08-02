using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnDefaultCommandReceived(Message message)
        {
            _logger.LogInformation($"Receive command: {message.Text}");

            await _storageService.UpdateState(message.Chat.Id, message.From.Username, message.Text[1..]);

            await OnMessageReceived(message);
        }
    }
}