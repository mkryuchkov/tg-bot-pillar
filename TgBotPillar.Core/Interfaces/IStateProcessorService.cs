using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Core.Interfaces
{
    public interface IStateProcessorService
    {
        Task<State> GetStartStateAsync();

        Task<State> GetStateAsync(string stateName);
    }
}