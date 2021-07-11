using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TgBotPillar.Api.Services;

namespace TgBotPillar.Api.Controllers
{
    public class WebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] HandleUpdateService handleUpdateService,
            [FromBody] Update update)
        {
            if (update == null) {
                return BadRequest();
            }
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}