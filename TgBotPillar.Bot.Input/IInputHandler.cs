using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input
{
    internal interface IInputHandler
    {
        string Name { get; }
        Task<string> HandleAsync(IDialogContext context);
    }
}