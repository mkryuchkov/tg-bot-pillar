using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IInputHandlersManager
    {
        Task HandleAsync(string name, DialogContext context);
    }
}