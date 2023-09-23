using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using tec_xx.Commands;

namespace tec_xx
{
    public class Bot
    {
        #region Roles
        private DiscordRole pillTossers;
        private DiscordRole medSchoolStudents;
        private DiscordRole patients;

        private DiscordRole northAmerica;
        private DiscordRole europe;
        private DiscordRole asiaAustralia;
        private DiscordRole southCentralAmerica;

        private DiscordRole whiteAlt;
        private DiscordRole greenAlt;
        private DiscordRole blueAlt;
        private DiscordRole redAlt;
        private DiscordRole yellowAlt;
        private DiscordRole blackAlt;
        private DiscordRole pinkAlt;
        private DiscordRole purpleAlt;

        private DiscordRole heHim;
        private DiscordRole sheHer;
        private DiscordRole theyThem;
        #endregion

        private static DiscordChannel flairChannel;

        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        private static string[] activityMessages = new string[4] { "Dreaming in digital", "Living in realtime", "Thinking in binary", "Talking in IP" };

        private Timer timer;

        private List<string> playlistLinks = new List<string>();

        private static Random rndWelcome = new Random();

        private static Random rndJoined = new Random();

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            {
                using var sr = new StreamReader(fs, new UTF8Encoding(false));
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            string discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            if (string.IsNullOrEmpty(discordToken))
            {
                throw new InvalidOperationException("Discord token is not set in the environment variables!");
            }

            var config = new DiscordConfiguration
            {
                Intents = DiscordIntents.All,
                Token = discordToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            };

            timer = new Timer()
            {
                AutoReset = true,
                Interval = 10000
            };

            Client = new DiscordClient(config);
            Client.GuildAvailable += OnClientReady;
            Client.ComponentInteractionCreated += AssignRoles;
            


            timer.Elapsed += SetActivityStatus;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<CharacterCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private async Task AssignRoles(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

            var member = e.Guild.Members[e.User.Id];

            if (e.Values.Length > 0)
            {
                if (e.Values[0].StartsWith("mr"))
                {
                    await ClearMainRoles(member);

                    switch (e.Values[0])
                    {
                        case "mr_mains":
                            await member.GrantRoleAsync(pillTossers);
                            break;
                        case "mr_pockets":
                            await member.GrantRoleAsync(medSchoolStudents);
                            break;
                        case "mr_visitors":
                            await member.GrantRoleAsync(patients);
                            break;
                    }
                }
                else if (e.Values[0].StartsWith("mk"))
                {
                    await ClearMatchmakingRoles(member);

                    foreach (var val in e.Values)
                    {
                        switch (val)
                        {
                            case "mk_northAmerica":
                                await member.GrantRoleAsync(northAmerica);
                                break;
                            case "mk_europe":
                                await member.GrantRoleAsync(europe);
                                break;
                            case "mk_asiaAustralia":
                                await member.GrantRoleAsync(asiaAustralia);
                                break;
                            case "mk_southCentralAmerica":
                                await member.GrantRoleAsync(southCentralAmerica);
                                break;
                        }
                    }
                }
                else if (e.Values[0].StartsWith("alt"))
                {
                    await ClearAltRoles(member);

                    foreach (var val in e.Values)
                    {
                        switch (val)
                        {
                            case "alt_white":
                                await member.GrantRoleAsync(whiteAlt);
                                break;
                            case "alt_green":
                                await member.GrantRoleAsync(greenAlt);
                                break;
                            case "alt_blue":
                                await member.GrantRoleAsync(blueAlt);
                                break;
                            case "alt_red":
                                await member.GrantRoleAsync(redAlt);
                                break;
                            case "alt_yellow":
                                await member.GrantRoleAsync(yellowAlt);
                                break;
                            case "alt_black":
                                await member.GrantRoleAsync(blackAlt);
                                break;
                            case "alt_pink":
                                await member.GrantRoleAsync(pinkAlt);
                                break;
                            case "alt_purple":
                                await member.GrantRoleAsync(purpleAlt);
                                break;
                        }
                    }
                }
                else if (e.Values[0].StartsWith("pr"))
                {
                    await ClearPronounsRoles(member);

                    switch (e.Values[0])
                    {
                        case "pr_him":
                            await member.GrantRoleAsync(heHim);
                            break;
                        case "pr_her":
                            await member.GrantRoleAsync(sheHer);
                            break;
                        case "pr_them":
                            await member.GrantRoleAsync(theyThem);
                            break;
                    }
                } 
            }
            else
            {
                switch (e.Id)
                {
                    case "dropdownRoleSelect":
                        await ClearMainRoles(member);
                        break;
                    case "dropdownMatchmakingRoleSelect":
                        await ClearMatchmakingRoles(member);
                        break;
                    case "dropdownAltColorRoleSelect":
                        await ClearAltRoles(member);
                        break;
                    case "dropdownPronounsSelect":
                        await ClearPronounsRoles(member);
                        break;
                }
            }
        }

        private async Task ClearMainRoles(DiscordMember member)
        {
            await member.RevokeRoleAsync(pillTossers);
            await member.RevokeRoleAsync(medSchoolStudents);
            await member.RevokeRoleAsync(patients);
        }

        private async Task ClearMatchmakingRoles(DiscordMember member)
        {
            await member.RevokeRoleAsync(northAmerica);
            await member.RevokeRoleAsync(europe);
            await member.RevokeRoleAsync(asiaAustralia);
            await member.RevokeRoleAsync(southCentralAmerica);
        }

        private async Task ClearAltRoles(DiscordMember member)
        {
            await member.RevokeRoleAsync(whiteAlt);
            await member.RevokeRoleAsync(greenAlt);
            await member.RevokeRoleAsync(blueAlt);
            await member.RevokeRoleAsync(redAlt); 
            await member.RevokeRoleAsync(yellowAlt);
            await member.RevokeRoleAsync(blackAlt);
            await member.RevokeRoleAsync(pinkAlt);
            await member.RevokeRoleAsync(purpleAlt);
        }

        private async Task ClearPronounsRoles(DiscordMember member)
        {
            await member.RevokeRoleAsync(heHim);
            await member.RevokeRoleAsync(sheHer);
            await member.RevokeRoleAsync(theyThem);
        }

        private async Task CheckForNewVideosInPlaylist()
        {
            foreach (string link in playlistLinks)
            {
            }
        }

        private async Task OnClientReady(DiscordClient client, GuildCreateEventArgs e)
        {
            //const Int32 bufferSize = 128;

            //using (var fileStream = File.OpenRead("PlaylistLinks.txt"))
            //{
            //    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
            //    {
            //        string line;
            //        while ((line = streamReader.ReadLine()) != null)
            //            playlistLinks.Add(line);
            //    }
            //}

            

            #region Roles Assignment
            pillTossers = Client.Guilds[125318974136123392].GetRole(202111502683996170);
            medSchoolStudents = Client.Guilds[125318974136123392].GetRole(202113348181950465);
            patients = Client.Guilds[125318974136123392].GetRole(202113663610388480);

            northAmerica = Client.Guilds[125318974136123392].GetRole(341904930124333056);
            europe = Client.Guilds[125318974136123392].GetRole(593227155362676856);
            asiaAustralia = Client.Guilds[125318974136123392].GetRole(593227152460218397);
            southCentralAmerica = Client.Guilds[125318974136123392].GetRole(720418637378486344);

            whiteAlt = Client.Guilds[125318974136123392].GetRole(535241884898164746);
            greenAlt = Client.Guilds[125318974136123392].GetRole(535241900131614733);
            blueAlt = Client.Guilds[125318974136123392].GetRole(535241899175575574);
            redAlt = Client.Guilds[125318974136123392].GetRole(535241898126868510);
            yellowAlt = Client.Guilds[125318974136123392].GetRole(535241902090485771);
            blackAlt = Client.Guilds[125318974136123392].GetRole(535241900844646411);
            pinkAlt = Client.Guilds[125318974136123392].GetRole(535242958425948160);
            purpleAlt = Client.Guilds[125318974136123392].GetRole(535242730708926496);

            heHim = Client.Guilds[125318974136123392].GetRole(891395259920175176);
            sheHer = Client.Guilds[125318974136123392].GetRole(891395538753314827);
            theyThem = Client.Guilds[125318974136123392].GetRole(891395850692091998);
            #endregion

            flairChannel = Client.Guilds[125318974136123392].GetChannel(202114898191319040);

            await SendMainRoleSelectMessage();
            await SendMatchmakingRoleMessage();
            await SendAltColorRoleMessage();
            await SendPronounsRoleMessage();


            timer.Start();

            return;
        }

        private async Task SendMainRoleSelectMessage()
        {
            // mr = Shortcut for Main Roles
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("Pill Tossers", "mr_mains", "Dr. Mario mains or secondaries, especially if you use Dr. Mario in tournament.", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":RedVirus:"))),
                new DiscordSelectComponentOption("Med School Students", "mr_pockets", "Dr. Mario pocket, or otherwise picking up the character.", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":YellowVirus:"))),
                new DiscordSelectComponentOption("Patients", "mr_visitors", "Non-users and visitors.", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":BlueVirus:")))
            };

            var dropdown = new DiscordSelectComponent("dropdownRoleSelect", null, options, false, 1, 1);

            await new DiscordMessageBuilder()
                .WithContent("In this channel, you may select for a role to represent your usage of Dr. Mario. The roles are as follows:")
                .AddComponents(dropdown)
                .SendAsync(flairChannel);
        }

        private async Task SendMatchmakingRoleMessage()
        {
            // Mk = Shoidkrtcut for Matchmaking
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("United States/Canada", "mk_northAmerica", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":Fair:"))),
                new DiscordSelectComponentOption("Europe", "mk_europe", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":Nair:"))),
                new DiscordSelectComponentOption("Asia/Australia", "mk_asiaAustralia", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":UpSmash:"))),
                new DiscordSelectComponentOption("South/Central America", "mk_southCentralAmerica", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":Bair:")))
            };

            var dropdown = new DiscordSelectComponent("dropdownMatchmakingRoleSelect", null, options, false, 0, 4);

            await new DiscordMessageBuilder()
                .WithContent("Please select an option if you would like the friendlies role for your region which grants access to #operating-room")
                .AddComponents(dropdown)
                .SendAsync(flairChannel);
        }

        private async Task SendAltColorRoleMessage()
        {
            // Alt = Shortcut for Alternative Skins
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("Default/White", "alt_white", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":DefaultDoc:"))),
                new DiscordSelectComponentOption("Green", "alt_green", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":GreenDoc:"))),
                new DiscordSelectComponentOption("Blue", "alt_blue", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":BlueDoc:"))),
                new DiscordSelectComponentOption("Red", "alt_red", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":RedDoc:"))),
                new DiscordSelectComponentOption("Yellow", "alt_yellow", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":YellowDoc:"))),
                new DiscordSelectComponentOption("Black", "alt_black", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":BlackDoc:"))),
                new DiscordSelectComponentOption("Pink", "alt_pink", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":PinkDoc:"))),
                new DiscordSelectComponentOption("Purple", "alt_purple", emoji: new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":PurpleDoc:")))
            };

            var dropdown = new DiscordSelectComponent("dropdownAltColorRoleSelect", null, options, false, 0, 8);

            await new DiscordMessageBuilder()
                .WithContent("Please select one of these options to receive a color flair for your alt color of choice:")
                .AddComponents(dropdown)
                .SendAsync(flairChannel);
        }

        private async Task SendPronounsRoleMessage()
        {
            // pr = Shortcut for Pronouns
            var options = new List<DiscordSelectComponentOption>()
            {
                new DiscordSelectComponentOption("He/Him", "pr_him"),
                new DiscordSelectComponentOption("She/Her", "pr_her"),
                new DiscordSelectComponentOption("They/Them", "pr_them")
            };

            var dropdown = new DiscordSelectComponent("dropdownPronounsSelect", null, options, false, 0, 1);

            await new DiscordMessageBuilder()
                .WithContent("You can choose a role with your pronouns here:")
                .AddComponents(dropdown)
                .SendAsync(flairChannel);
        }

        private async void SetActivityStatus(object source, ElapsedEventArgs e)
        {
            var activity = new DiscordActivity();

            var random = new Random();

            int index = random.Next(0, activityMessages.Length);

            activity.Name = activityMessages[index];

            await Client.UpdateStatusAsync(activity);
        }
    }
}