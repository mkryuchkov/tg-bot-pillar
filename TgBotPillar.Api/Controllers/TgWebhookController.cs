using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TgBotPillar.Core.Interfaces;

namespace TgBotPillar.Api.Controllers
{
    public class TgWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] IUpdateHandlerService<Update> updateHandlerService,
            [FromBody] Update update)
        {
            if (update == null)
            {
                return BadRequest();
            }

            await updateHandlerService.Handle(update);
            return Ok();
        }
    }
}