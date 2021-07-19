﻿using System.Threading.Tasks;
using TgBotPillar.Core.Model;

namespace TgBotPillar.Bot.Input.Handlers
{
    public class TextInputHandler : IInputHandler
    {
        public string Name => "text_input_handler";

        public Task<object> HandleAsync(DialogContext context)
        {
            return Task.FromResult<object>("Ok");
        }
    }
}