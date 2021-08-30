using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBotPillar.Bot.ModelExtensions;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot
{
    public partial class UpdateHandlerService
    {
        private async Task OnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            _logger.LogInformation($"Receive CallbackQuery {callbackQuery.Id}: {callbackQuery.Data}");

            var currentContext = await _storageService.GetContext(
                callbackQuery.Message.Chat.Id, callbackQuery.From.Username);
            var currentState = await _stateProcessor.GetState(currentContext.State);
            var newState = currentState;

            var callBackData = callbackQuery.Data.Split(':');
            if (callBackData.Length > 2 && callBackData[0] == "pager")
            {
                switch (callBackData[1])
                {
                    case "entry":
                        await _storageService.Stash(callbackQuery.Message.Chat.Id,
                            $"{currentState.Pager.EntryTransition}:question:type", callBackData[2]);
                        await _storageService.Stash(callbackQuery.Message.Chat.Id,
                            $"{currentState.Pager.EntryTransition}:question:id", callBackData[3]);

                        newState = await UpdateState(callbackQuery, currentState.Pager.EntryTransition);
                        break;

                    case "page":
                        await _storageService.Stash(callbackQuery.Message.Chat.Id,
                            $"{currentContext.State}:pager:{currentState.Pager.QuestionType}", 
                            int.Parse(callBackData[2]));
                        break;
                }
            }
            else
            {
                newState = await UpdateState(callbackQuery, callbackQuery.Data);
            }

            var newContext = await _storageService.GetContext(
                callbackQuery.Message.Chat.Id, callbackQuery.From.Username);

            if (newState.HasInlineMarkup())
            {
                await _botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId,
                    await newState.GetFormattedText(_inputHandlersManager, newContext),
                    ParseMode.MarkdownV2,
                    replyMarkup: await newState.GetInlineKeyboard(_inputHandlersManager, newContext, _storageService));
            }
            else
            {
                await _botClient.EditMessageTextAsync(
                    callbackQuery.Message.Chat.Id,
                    callbackQuery.Message.MessageId,
                    $"`>>>` _{currentState.Buttons.First(b => b.Transition == callbackQuery.Data).Label}_",
                    ParseMode.MarkdownV2,
                    replyMarkup: null);

                await _botClient.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    await newState.GetFormattedText(_inputHandlersManager, newContext),
                    ParseMode.MarkdownV2,
                    replyMarkup: await newState.GetKeyboard(_inputHandlersManager, newContext, _storageService));
            }
        }

        private async Task<State> UpdateState(CallbackQuery callbackQuery, string stateName)
        {
            await _storageService.UpdateState(callbackQuery.Message.Chat.Id, callbackQuery.From.Username, stateName);
            return await _stateProcessor.GetState(stateName);
        }
    }
}