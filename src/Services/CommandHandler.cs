using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace WatchDog.Services
{
    class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly Configuration _config;
        private readonly IServiceProvider _provider;

        public CommandHandler( DiscordSocketClient client, CommandService commandService, Configuration config, IServiceProvider provider)
        {
            _client = client;
            _commandService = commandService;
            _config = config;
            _provider = provider;

            _client.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMsg)
        {
            var msg = socketMsg as SocketUserMessage;

            if (msg == null)
            {
                return;
            }

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;

            if (msg.HasCharPrefix(_config.Prefix, ref argPos))
            {
                var result = await _commandService.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)     // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}
