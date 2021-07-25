using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input
{
    internal interface IInputHandler
    {
        string Name { get; }
        Task<string> Handle(
            IStorageService storageService,
            IDictionary<string, string> parameters,
            IDialogContext context,
            string text);
    }
}