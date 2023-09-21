using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System;
using DSharpPlus.Exceptions;
using DSharpPlus.EventArgs;

namespace tec_xx.Commands
{
    public class CharacterCommands : BaseCommandModule
    {
        private const string author = "This bot was created by Junes#9469";
        private const string docHeadUrl = "https://www.ssbwiki.com/images/c/c8/DrMarioHeadSSBUWebsite.png";
        private const string docStop = "https://docs.google.com/spreadsheets/d/1f00Ph9bDgF5Nc0GFokGsmuTsRUnBmLRj8aN-yeXPy4g/edit?usp=sharing";
        private const string footer = "The tips were given by Junes#9469 and/or Labrys#4412 on Discord.\nPlease contact us or the mod team if you have any suggestions!";
        private const string pastebin = "https://pastebin.com/8Aw08aBv";

        [Command("vs")]
        public async Task SendMatchUpTips(CommandContext ctx, string character)
        {
            try
            {
                var headIconFileName = new List<string>();
                var advice = new MatchupAdvice();
                var alias = new Aliases();

                var advices = JsonConvert.DeserializeObject<List<MatchupAdvice>>(File.ReadAllText("MatchupAdvice.json"));
                var aliases = JsonConvert.DeserializeObject<List<Aliases>>(File.ReadAllText("Aliases.json"));

                character = character.ToLower();

                advice = advices.Find(x => x.Character == character);

                if (advice == null)
                {
                    alias = aliases.Find(x => x.Alias == character);

                    if (alias != null)
                    {
                        advice = advices.Find(x => x.Character == alias.Character);
                        character = advice.Character;
                    }
                    else
                    {
                        var msg = new DiscordMessageBuilder()
                            .WithContent($"You didn't enter a valid character argument. \n For a comprehensive list about all character commands, please see this {pastebin} link.")
                            .SendAsync(ctx.Channel);

                        return;
                    }

                }

                string headIconDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HeadIcons");

                Console.WriteLine(character);

                // Robin already contains the word "rob"
                if (string.Equals(character, "rob"))
                {
                    headIconFileName.Add(string.Concat(headIconDirectory, @"/ROBHeadSSBUWebsite.png"));
                }
                else if (string.Equals(character, "samus"))
                {
                    headIconFileName.Add(string.Concat(headIconDirectory, @"/SamusHeadSSBUWebsite.png"));
                }
                else if (string.Equals(character, "falco"))
                {
                    headIconFileName.Add(string.Concat(headIconDirectory, @"/FalcoHeadSSBUWebsite.png"));
                }
                else if (string.Equals(character, "bowser"))
                {
                    headIconFileName.Add(string.Concat(headIconDirectory, @"/BowserHeadSSBUWebsite.png"));

                }
                else
                {
                    headIconFileName = Directory.GetFiles(headIconDirectory).Where(s => s.ToLower().Contains(character)).ToList();
                }

                using (var fs = new FileStream(headIconFileName[0], FileMode.Open))
                {
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = $"Dr. Mario vs. {advice.Title}",
                        Url = docStop,
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {
                            IconUrl = docHeadUrl,
                            Name = author
                        },
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = docHeadUrl,
                            Text = footer
                        },
                        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                        {
                            Url = $"attachment://{Path.GetFileName(headIconFileName[0])}"
                        },

                        Color = new Optional<DiscordColor>(new DiscordColor(advice.Color)),
                    };

                    if (!string.IsNullOrEmpty(advice.SecondaryNotes))
                    {
                        string[] notesTitles = ReturnNotesTitle(advice);

                        embed.AddField(notesTitles[0], BuildNotes(advice));

                        embed.AddField(notesTitles[1], string.Concat("```\n", advice.SecondaryNotes, "```"));

                        if (!string.IsNullOrEmpty(advice.TertiaryNotes))
                            embed.AddField(notesTitles[2], string.Concat("```\n", advice.TertiaryNotes, "```"));
                    }
                    else
                    {
                        embed.AddField("Notes", BuildNotes(advice));
                    }

                    embed.AddField("Stages (ordered from best to worst)", BuildStages(advice));
                    embed.AddField("Documents", BuildOtherFields(advice.Document));
                    embed.AddField("Videos", BuildOtherFields(advice.Video));
                    embed.AddField("Other", BuildOtherFields(advice.Other));

                    var msg = await new DiscordMessageBuilder()
                            .WithEmbed(embed)
                            .WithFiles(new Dictionary<string, Stream>() { { $"{Path.GetFileName(headIconFileName[0])}", fs } })
                            .SendAsync(ctx.Channel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private static string BuildNotes(MatchupAdvice notes)
        {
            var sb = new StringBuilder("```");

            if (!string.IsNullOrEmpty(notes.PrimaryNotes))
            {
                sb.Append(notes.PrimaryNotes);
            }
            else
            {
                return "---WORK IN PROGRESS---";
            }

            sb.Append("```");

            return sb.ToString();
        }

        private static string BuildStages(MatchupAdvice stages)
        {
            if (!string.IsNullOrEmpty(stages.WinningStages))
            {
                return string.Concat("```", stages.WinningStages, "\n", stages.NeutralStages, stages.LosingStages, "```");
            }
            else
            {
                return "```/```";
            }
        }

        private static string BuildOtherFields(string txt)
        {
            if (!string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            else
            {
                return "/";
            }
        }

        private static string[] ReturnNotesTitle(MatchupAdvice advice)
        {
            switch (advice.Title)
            {
                case "(Dark) Pit":
                    return new string[] { "Pit", "Dark Pit" };

                case "Pokémon Trainer":
                    return new string[] { "Squirtle", "Ivysaur", "Charizard" };

                case "Ryu":
                    return new string[] { "Ryu", "Ken" };

                default:
                    return new string[] { " -Unknown " };
            }
        }
    }

    public class MatchupAdvice
    {
        public string Character { get; set; }
        public string Color { get; set; }
        public string Document { get; set; }
        public string Video { get; set; }
        public string WinningStages { get; set; }
        public string NeutralStages { get; set; }
        public string LosingStages { get; set; }
        public string Other { get; set; }
        public string PrimaryNotes { get; set; }
        public string SecondaryNotes { get; set; }
        public string TertiaryNotes { get; set; }
        public string Title { get; set; }
    }

    public class Aliases
    {
        public string Alias { get; set; }
        public string Character { get; set; }
    }
}