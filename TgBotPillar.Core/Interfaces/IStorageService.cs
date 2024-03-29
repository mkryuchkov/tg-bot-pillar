using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStorageService
    {
        Task<IDialogContext> GetContext(long chatId, string userName);
        
        Task UpdateState(long chatId, string userName, string stateName);
        
        Task UpdateUserFlags(string userName, IList<string> flags);

        Task SetUserFlag(string userName, string flag);
        
        Task<string> SaveQuestion(long chatId, string questionType, string text);
        
        Task<IQuestion> GetRandomQuestion(long chatId, string questionType);
        
        Task<IQuestion> GetQuestion(long chatId, string questionType, string questionId);
        
        Task<Tuple<IEnumerable<IQuestion>, long>> GetQuestions(long chatId, string questionType,
            int pageSize, int pageNumber);
        
        Task Stash<TValue>(long chatId, string key, TValue value);
        
        Task<TValue> UnStash<TValue>(long chatId, string key);
        
        Task SaveAnswer(long chatId, string questionType, string questionId, string text);
    }
}