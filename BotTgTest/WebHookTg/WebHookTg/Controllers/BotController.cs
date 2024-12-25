using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebHookTg.Controllers
{
    [ApiController]
    [Route("api/bot")]
    public class BotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            var bot = new TelegramBotClient("7184850117:AAEZqnykD42xoq2g5RuS73pxjXqCOAXGnFw");
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                await bot.SetWebhook("https://your.public.host:port/bot", allowedUpdates: []);
            }
            return Ok();
        }
    }
}
