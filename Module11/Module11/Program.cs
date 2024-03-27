using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module11;
using System;
using System.Text;
using Telegram.Bot;
using Module11.Controllers;
using Module11.Services;
using Module11.Configuration;

namespace Module11
{
    class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder() 
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                .UseConsoleLifetime()
                .Build();

            Console.WriteLine("Servives launch");

            await host.RunAsync();
            Console.WriteLine("Services stop");
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<ITelegramBotClient>(provide => new TelegramBotClient(appSettings.BotToken));
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddHostedService<Bot>();

            services.AddTransient<DefaultMessage>();
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<VoiceMessageController>();

            services.AddSingleton<IFileHandler, AudioFileHandler>();
        }

        static AppSettings BuildAppSettings() 
        {
            return new AppSettings()
            {
                BotToken = "6387106207:AAGdx-fhW3M4pmK2qyMxXwP5Q0Gd6uMXNF8",
                DownloadsFolder = @"C:\Users\Ghosman\Downloads",
                AudioFileName = "audio",
                InputAudioFormat = "ogg",
                OutputAudioFormat = "wav",
                InputAudioBitrate = 48000,
            };
        }
    }
}