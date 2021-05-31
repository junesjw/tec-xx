using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using DSharpPlus.CommandsNext.Exceptions;
using tec_xx.Commands;

namespace tec_xx
***REMOVED***
    public class Bot
    ***REMOVED***
        public DiscordClient Client ***REMOVED*** get; private set; ***REMOVED***
        public CommandsNextExtension Commands ***REMOVED*** get; private set; ***REMOVED***

        public async Task RunAsync()
        ***REMOVED***
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            ***REMOVED***
                using var sr = new StreamReader(fs, new UTF8Encoding(false));
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
          ***REMOVED***

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            ***REMOVED***
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
          ***REMOVED***;

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            ***REMOVED***
                StringPrefixes = new string[] ***REMOVED*** configJson.Prefix ***REMOVED***,
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
          ***REMOVED***;

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<CharacterCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
      ***REMOVED***

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        ***REMOVED***
            return Task.CompletedTask;
      ***REMOVED***


  ***REMOVED***
***REMOVED***