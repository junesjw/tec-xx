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

        private List<string> playlistLinks = new List<string>();

        static Random rndWelcome = new Random();

        static Random rndJoined = new Random();

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
                Intents = DiscordIntents.All,
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

        private async Task CheckForNewVideosInPlaylist()
        ***REMOVED***
            foreach (string link in playlistLinks)
            ***REMOVED***

          ***REMOVED***
      ***REMOVED***

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        ***REMOVED***
            //const Int32 bufferSize = 128;

            //using (var fileStream = File.OpenRead("PlaylistLinks.txt"))
            //***REMOVED***
            //    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
            //    ***REMOVED***
            //        string line;
            //        while ((line = streamReader.ReadLine()) != null)
            //            playlistLinks.Add(line);
            //  ***REMOVED***
            //***REMOVED***

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