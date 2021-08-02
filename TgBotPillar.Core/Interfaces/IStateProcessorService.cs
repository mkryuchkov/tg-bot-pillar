using System.Threading.Tasks;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStateProcessorService
    {
        Task<State> GetState(string stateName);
    }
}