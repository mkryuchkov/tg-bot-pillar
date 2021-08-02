using System.Collections.Generic;
using System.Linq;
using TgBotPillar.Core.Interfaces;
using TgBotPillar.Core.Model;
using TgBotPillar.Core.Scheme;

namespace TgBotPillar.Bot.ModelExtensions
{
    public static class HasVisibilityHandlerExtensions
    {
        public static IEnumerable<T> FilterVisible<T>(this IEnumerable<T> elements,
            IInputHandlersManager handlersManager, IDialogContext context
        ) where T : IHasVisibilityHandler
        {
            return elements
                .Select(async option => new
                {
                    Value = option,
                    IsVisible = option.Visibility == null ||
                                (await handlersManager.Handle(option.Visibility, context))
                                .ToLowerInvariant() == "true"
                })
                .Where(_ => _.Result.IsVisible)
                .Select(_ => _.Result.Value);
        }
    }
}