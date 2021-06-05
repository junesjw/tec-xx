﻿using DSharpPlus;
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

namespace tec_xx
***REMOVED***
    public class Bot
    ***REMOVED***
        public DiscordClient Client ***REMOVED*** get; private set; ***REMOVED***
        public CommandsNextExtension Commands ***REMOVED*** get; private set; ***REMOVED***

        private static string[] activityMessages = new string[4] ***REMOVED*** "Dreaming in digital", "Living in realtime", "Thinking in binary", "Talking in IP" ***REMOVED***;

        private Timer timer;

        public async Task RunAsync()
        ***REMOVED***
            var json = string.Empty;
            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            using (var fs = File.OpenRead("config.json"))
            ***REMOVED***
                using var sr = new StreamReader(fs, new UTF8Encoding(false));
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
          ***REMOVED***

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