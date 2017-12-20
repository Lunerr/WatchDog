using Discord.Commands;
using System.Threading.Tasks;
using WatchDog.Services;

namespace WatchDog.Events
{
    public class CommandExecuted
    {
        private readonly CommandService _commandService;
        private readonly Sender _sender;

        public CommandExecuted(CommandService commandService, Sender sender)
        {
            _commandService = commandService;
            _sender = sender;

            _commandService.CommandExecuted += OnCommandExecutedAsync;
        }

        private Task OnCommandExecutedAsync(CommandInfo command, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                var message = string.Empty;

                switch (result.Error.Value)
                {
                    case CommandError.UnknownCommand:
                    //return;
                    default:
                        message = result.ErrorReason;
                        break;
                }

                return _sender.SendAsync(context.Channel, message);
            }

            return Task.CompletedTask;
        }
    }
}
