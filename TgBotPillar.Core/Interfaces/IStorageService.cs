using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStorageService
    {
        Task<IDialogContext> GetContext(long chatId);
        
        Task UpdateState(long chatId, string stateName);
    }
}