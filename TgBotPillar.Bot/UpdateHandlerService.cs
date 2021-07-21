using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService : IUpdateHandlerService<Update>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandlerService> _logger;
        private readonly IStateProcessorService _stateProcessor;
        private readonly IInputHandlersManager _inputHandlersManager;
        private readonly IStorageService _storageService;

        public UpdateHandlerService(
            ITelegramBotClient botClient,
            ILogger<UpdateHandlerService> logger,
            IStateProcessorService stateProcessor,
            IInputHandlersManager inputHandlersManager,
            IStorageService storageService)
        {
            _botClient = botClient;
            _logger = logger;
            _stateProcessor = stateProcessor;
            _inputHandlersManager = inputHandlersManager;
            _storageService = storageService;
        }

        public async Task Handle(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => OnMessageReceived(update.Message),
                UpdateType.CallbackQuery => OnCallbackQueryReceived(update.CallbackQuery),
                _ => UnknownUpdateHandler(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleError(exception);
            }
        }
    }
}