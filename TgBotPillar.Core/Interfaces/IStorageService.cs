using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStorageService
    {
        Task<IDialogContext> GetContext(long chatId, string userName);
        
        Task UpdateState(long chatId, string userName, string stateName);
        
        Task SaveQuestion(long chatId, string questionType, string text);
        
        Task<IQuestion> GetQuestion(long chatId, string questionType);
        
        Task Stash<TValue>(long chatId, string key, TValue value);
        
        Task<TValue> UnStash<TValue>(long chatId, string key);
        
        Task SaveAnswer(long chatId, string questionType, string questionId, string text);
    }
}