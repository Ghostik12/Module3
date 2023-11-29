using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Module11.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"Русский", $"ru"),
                        InlineKeyboardButton.WithCallbackData($"Английский", $"en")
                    });

                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот превращает аудио в текст.</b>{Environment.NewLine}" 
                        + $"{Environment.NewLine} Можно записать сообщение и переслать другу, если лень печатать", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                default:
                    await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Отправте аудио для превращения в текст", cancellationToken: ct);
                    break;
            }
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
        }
    }
}
