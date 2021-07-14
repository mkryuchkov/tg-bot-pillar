using System.Collections.Generic;
using System.Threading.Tasks;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStateProcessorService
    {
        Task Initialization { get; }
        
        Task<object> Process(object context, object update);
    }
}