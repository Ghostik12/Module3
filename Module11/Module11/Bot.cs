using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Module11.Controllers;

namespace Module11
{
    class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;
        private InlineKeyboardController _inlinekeyboardController;
        private VoiceMessageController _voiceMessageController;
        private TextMessageController _textMessageController;
        private DefaultMessage _defaultMessage;

        public Bot (ITelegramBotClient telegramClient, VoiceMessageController voiceMessageController, TextMessageController textMessageController, DefaultMessage defaultMessage, InlineKeyboardController inlineKeyboardController)
        {
            _telegramClient = telegramClient;
            _voiceMessageController = voiceMessageController;
            _textMessageController = textMessageController;
            _defaultMessage = defaultMessage;
            _inlinekeyboardController = inlineKeyboardController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions() { AllowedUpdates = { } }, cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }

        async Task HandleUpdateAsync (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlinekeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type) 
                {
                    case MessageType.Voice:
                        await _voiceMessageController.Handle(update.Message, cancellationToken);
                        return;
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default:
                        await _defaultMessage.Handle(update.Message,cancellationToken);
                        return;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException}]\n{apiRequestException}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}
