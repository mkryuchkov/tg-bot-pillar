using System;
using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStateProcessorService
    {
        Task<State> GetStateAsync(string stateName);
        
        Task<Tuple<string, State>> GetNewStateAsync(string state, string inputText);
    }
}