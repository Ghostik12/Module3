using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Tgweb.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class BotController : ControllerBase
    {
        private const string _token = "7636151838:AAF9FcP9-WnHoE8SJmYD-bEQNEObULfsyfs";
        private readonly HttpClient _httpClient;

        public BotController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            try
            {
                long chatId = update.Message.Chat.Id;

                if (update.Message.Voice != null)
                {
                    await HandleVoiceMessage(update, chatId);
                }
                else if (update.Message.Audio != null)
                {
                    await HandleAudioMessage(update, chatId);
                }
                else
                {
                    string messageText = update.Message.Text;
                    await RespondAsync(chatId, $"Вы написали: {messageText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return Ok();
        }

        private async Task RespondAsync(long chatId, string response)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"https://api.telegram.org/bot{_token}/sendMessage";
                var json = JsonConvert.SerializeObject(new { chat_id = chatId, text = response });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await httpClient.PostAsync(requestUri, content);
            }
        }

        private async Task HandleVoiceMessage([FromBody] Update update, long chatId)
        {
            var voiceFileId = update.Message.Voice.FileId;
            var filePath = await DownloadFile(voiceFileId, "ogg");
            await SendVoiceAsync(chatId, filePath);
        }

        private async Task HandleAudioMessage(Update update, long chatId)
        {
            var audioFileId = update.Message.Audio.FileId;
            var filePath = await DownloadFile(audioFileId, "mp3");
            await SendAudioAsync(chatId, filePath);
        }

        private async Task<string> DownloadFile(string fileId, string fileType)
        {
            var fileInfoResponse = await _httpClient.GetAsync($"https://api.telegram.org/bot{_token}/getFile?file_id={fileId}");
            var fileInfo = JsonConvert.DeserializeObject<JObject>(await fileInfoResponse.Content.ReadAsStringAsync());
            var filePath = (string)fileInfo["result"]["file_path"];

            var downloadUrl = $"https://api.telegram.org/file/bot{_token}/{filePath}";
            var downloadResponse = await _httpClient.GetAsync(downloadUrl);
            if (!downloadResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Не удалось загрузить файл: {downloadResponse.StatusCode} - {downloadResponse.ReasonPhrase}");
            }
            var fileName = $"{Guid.NewGuid()}.{fileType}";
            var filePathLocal = Path.Combine(Path.GetTempPath(), fileName);

            using (var fs = new FileStream(filePathLocal, FileMode.CreateNew))
            {
                await downloadResponse.Content.CopyToAsync(fs);
            }

            return filePathLocal;
        }

        private async Task SendVoiceAsync(long chatId, string filePath)
        {
            using (var formData = new MultipartFormDataContent())
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    formData.Add(new StreamContent(fs), "voice", Path.GetFileName(filePath));
                }

                var requestUri = $"https://api.telegram.org/bot{_token}/sendVoice";
                var response = await _httpClient.PostAsync(requestUri, formData);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }


        }

        private async Task SendAudioAsync(long chatId, string filePath)
        {
            using (var formData = new MultipartFormDataContent())
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    formData.Add(new StreamContent(fs), "audio", Path.GetFileName(filePath));
                }

                var requestUri = $"https://api.telegram.org/bot{_token}/sendAudio";
                var response = await _httpClient.PostAsync(requestUri, formData);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
