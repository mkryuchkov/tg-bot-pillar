using System.Collections.Generic;

namespace TgBotPillar.Core.Model
{
    public interface IDialogContext
    {
        public long ChatId { get; }
        
        public string UserName { get; }
        
        string State { get; }

        IList<string> Flags { get; }
    }
}