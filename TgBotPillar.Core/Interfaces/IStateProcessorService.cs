using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStateProcessorService
    {
        Task<State> GetState(string stateName);
    }
}