using Discord;
using System;
using System.Threading.Tasks;

namespace WatchDog.Services
{
    public sealed class Logger
    {
        public Task LogAsync(LogSeverity severity, string message)
        {
            return Console.Out.WriteLineAsync($"{DateTime.UtcNow.ToString("hh:mm:ss")} [{severity}] {message}");
        }
    }
}
