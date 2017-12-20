using System;
using System.IO;
using System.Threading.Tasks;

namespace WatchDog.Utility
{
    public static class Arguments
    {
        public static async Task<string[]> ParseAsync(string[] args)
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
                    await ExitAsync();
                }
            }

            if (!File.Exists(configFile))
            {
                await Error("\nThe " + configFile + " does not exist.");
            }
            else if (!File.Exists(credentialsFile))
            {
                await Error("\nThe " + credentialsFile + " does not exist.");
            }

            return new string[] { await File.ReadAllTextAsync(configFile), await File.ReadAllTextAsync(credentialsFile) };
        }

        private static async Task Error(string message)
        {
            // TODO: Better way to handle console colors
            Console.BackgroundColor = ConsoleColor.DarkRed;
            await Console.Out.WriteLineAsync(message);
            Console.BackgroundColor = ConsoleColor.Black;
            await ExitAsync();
        }

        private static async Task ExitAsync()
        {
            await Console.Out.WriteLineAsync("\nUsage: dotnet WatchBot.dll [options]\n\n" +
                              "Options:\n" +
                              "  -c, --conf     The configuration file.\n" +
                              "  -C, --creds    The credentials file.\n\n" +
                              "Defaults:\n" +
                              "  -c, --conf     Configuration.json\n" +
                              "  -C, --creds    Credentials.json\n");

#if DEBUG
            Console.Read();
#endif

            Environment.Exit(-1);
        }
    }
}
