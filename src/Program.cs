using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WatchDog.Services;

namespace WatchDog
{
    class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync(args).GetAwaiter().GetResult();

        public async Task StartAsync(string[] args)
        {
            var parsedArgs = await ParseArguments(args);
            var config = JsonConvert.DeserializeObject<Configuration>(parsedArgs.Item1);
            var credentials = JsonConvert.DeserializeObject<Credentials>(parsedArgs.Item2);
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 10
            });
            var commandService = new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Verbose
            });

            var services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commandService)
                .AddSingleton<Logger>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode())))
                .AddSingleton(config)
                .AddSingleton(credentials);

            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<Logger>();

            await client.LoginAsync(TokenType.Bot, credentials.Token);
            await client.StartAsync();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            await Task.Delay(-1);
        }

        private static async Task<Tuple<string, string>> ParseArguments(string[] args)
        {
            var configFile = "Configuration.json";
            var credentialsFile = "Credentials.json";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-c" || args[i] == "--conf")
                {
                    configFile = args[i + 1];
                    i++;
                }
                else if (args[i] == "-C" || args[i] == "--creds")
                {
                    credentialsFile = args[i + 1];
                    i++;
                }
                else
                {
                    await Console.Out.WriteLineAsync("\nUnknown argument: " + args[i]);
                    await Exit();
                }
            }

            if (!File.Exists(configFile))
            {
                await Console.Out.WriteLineAsync("\nThe " + configFile + " does not exist.");
                await Exit();
            }
            else if (!File.Exists(credentialsFile))
            {
                await Console.Out.WriteLineAsync("\nThe " + credentialsFile + " does not exist.");
                await Exit();
            }

            return Tuple.Create(await File.ReadAllTextAsync(configFile), await File.ReadAllTextAsync(credentialsFile));
        }

        private static async Task Exit()
        {
            await Console.Out.WriteLineAsync("\nUsage: dotnet WatchBot.dll [options]\n\n" +
                              "Options:\n" +
                              "  -c, --conf     The configuration file.\n" +
                              "  -C, --creds    The credentials file.\n\n" +
                              "Defaults:\n" +
                              "  -c Configuration.json\n" +
                              "  -C Credentials.json\n");

            Environment.Exit(-1);
        }
    }
}
