using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using DSharpPlus.CommandsNext.Exceptions;
using tec_xx.Commands;
using DSharpPlus.Entities;
using System;
using System.Timers;
using System.Collections.Generic;

namespace tec_xx
***REMOVED***
    public class Bot
    ***REMOVED***
        public DiscordClient Client ***REMOVED*** get; private set; ***REMOVED***
        public CommandsNextExtension Commands ***REMOVED*** get; private set; ***REMOVED***

        private static string[] activityMessages = new string[4] ***REMOVED*** "Dreaming in digital", "Living in realtime", "Thinking in binary", "Talking in IP" ***REMOVED***;

        private Timer timer;

        static Random rnd = new Random();

        private DiscordChannel lounge;

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

            timer = new Timer()
            ***REMOVED***
                AutoReset = true,
                Interval = 10000
          ***REMOVED***;

            Client = new DiscordClient(config);
            Client.Ready += OnClientReady;
            Client.GuildMemberAdded += WelcomeMessage;

            timer.Elapsed += SetActivityStatus;
            
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

        private async Task WelcomeMessage(DiscordClient sender, GuildMemberAddEventArgs e)
        ***REMOVED***
            DiscordGuild guild = await Client.GetGuildAsync(689975663339634736);

            var channel = guild.GetChannel(689975663339634897);
            var welcomeMsgs = new List<string>();

            using (StreamReader reader = new StreamReader("WelcomeMessages.txt"))
            ***REMOVED***
                string line;

                while ((line = reader.ReadLine()) != null)
                ***REMOVED***
                    welcomeMsgs.Add(line);
              ***REMOVED***
          ***REMOVED***

            int r = rnd.Next(welcomeMsgs.Count);

            var msg = new DiscordMessageBuilder()
                .WithContent($"***REMOVED***e.Member.Mention***REMOVED*** ***REMOVED***welcomeMsgs[r]***REMOVED***")
                .SendAsync(channel);
      ***REMOVED***

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        ***REMOVED***
            timer.Start();
            return Task.CompletedTask;
      ***REMOVED***

        private async void SetActivityStatus(object source, ElapsedEventArgs e)
        ***REMOVED***
            var activity = new DiscordActivity();

            var random = new Random();

            int index = random.Next(0, activityMessages.Length);

            activity.Name = activityMessages[index];

            await Client.UpdateStatusAsync(activity);
      ***REMOVED***

  ***REMOVED***
***REMOVED***