using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Module11.Configuration;
using Module11.Services;

namespace Module11.Controllers
{
    public class VoiceMessageController
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly AppSettings _appSettings;
        private readonly IFileHandler _audioFileHandler;
        private readonly IStorage _memoryStorage;

        public VoiceMessageController(ITelegramBotClient telegramBotClient, AppSettings appSettings, IFileHandler audioFileHandler, IStorage memoryStorage)
        {
            _telegramBotClient = telegramBotClient;
            _appSettings = appSettings;
            _audioFileHandler = audioFileHandler;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var fileId = message.Voice?.FileId;
            if (fileId == null)
                return;

            await _audioFileHandler.Dowmload(fileId, ct);

            string userLanguageCode = _memoryStorage.GetSession(message.Chat.Id).LanguageCode;
            var result = _audioFileHandler.Process(userLanguageCode);
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
        }
    }
}
