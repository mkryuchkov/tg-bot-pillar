using System.Threading.Tasks;

namespace TgBotPillar.Core.Interfaces
{
    public interface IUpdateHandlerService<in T>
    {
        Task HandleAsync(T update);
    }
}