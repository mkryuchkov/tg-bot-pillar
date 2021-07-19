using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStorageService
    {
        Task<IDialogContext> GetContextAsync(long chatId);
        
        Task UpdateStateAsync(long chatId, string stateName);
    }
}