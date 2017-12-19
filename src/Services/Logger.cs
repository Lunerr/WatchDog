using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace WatchDog.Services
{
    class Logger
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        
        public Logger(DiscordSocketClient client, CommandService commandService)
        {
            _client = client;
            _commandService = commandService;

            _client.Log += OnLogAsync;
            _commandService.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            return Log(msg.Severity, msg.Source, msg.Exception?.Message ?? msg.Message);
        }

        public Task Log(LogSeverity severity, string source, string message)
        {
            return Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [{severity}] {source}: {message}");
        }
    }
}
