using System.Threading.Tasks;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Core.Interfaces
{
    public interface IInputHandlersManager
    {
        Task<string> Handle(Input input, IDialogContext context, string text = null);
        
        Task<string> Handle(Handler handler, IDialogContext context, string text = null);
    }
}