using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using WatchDog.Services;

namespace WatchDog.Events
{
    public class ClientLog
    {
        private readonly DiscordSocketClient _client;
        private readonly Logger _logger;

        public ClientLog(DiscordSocketClient client, Logger logger)
        {
            _client = client;
            _logger = logger;

            _client.Log += OnClientLogAsync;
        }

        private Task OnClientLogAsync(LogMessage msg)
        {
            return _logger.LogAsync(msg.Severity, msg.Source + ": " + (msg.Exception?.ToString() ?? msg.Message));
        }
    }
}
