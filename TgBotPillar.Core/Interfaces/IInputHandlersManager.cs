using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IInputHandlersManager
    {
        Task<string> HandleAsync(Input input, IDialogContext context, string text = null);
    }
}