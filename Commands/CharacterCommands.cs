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

namespace tec_xx.Commands
***REMOVED***
    public class CharacterCommands : BaseCommandModule
    ***REMOVED***
        private const string author = "This bot was created by Junes#9469";
        private const string docHeadUrl = "https://www.ssbwiki.com/images/c/c8/DrMarioHeadSSBUWebsite.png";
        private const string docStop = "https://docs.google.com/spreadsheets/d/1f00Ph9bDgF5Nc0GFokGsmuTsRUnBmLRj8aN-yeXPy4g/edit?usp=sharing";
        private const string footer = "The tips were given by Junes#9469 and/or Labrys#4412 on Discord.\nPlease contact us or the mod team if you have any suggestions!";
        private const string pastebin = "https://pastebin.com/8Aw08aBv";

        [Command("vs")]
        public async Task SendMatchUpTips(CommandContext ctx, string character)
        ***REMOVED***
            var headIconFileName = new List<string>();
            var advice = new MatchupAdvice();
            var alias = new Aliases();

            var advices = JsonConvert.DeserializeObject<List<MatchupAdvice>>(File.ReadAllText("MatchupAdvice.json"));
            var aliases = JsonConvert.DeserializeObject<List<Aliases>>(File.ReadAllText("Aliases.json"));

            character = character.ToLower();

            advice = advices.Find(x => x.Character == character);

            if (advice == null)
            ***REMOVED***
                alias = aliases.Find(x => x.Alias == character);

                if (alias != null)
                ***REMOVED***
                    advice = advices.Find(x => x.Character == alias.Character);
                    character = advice.Character;
              ***REMOVED***
                else
                ***REMOVED***
                    var msg = new DiscordMessageBuilder()
                        .WithContent($"You didn't enter a valid character argument. \n For a comprehensive list about all character commands, please see this ***REMOVED***pastebin***REMOVED*** link.")
                        .SendAsync(ctx.Channel);

                    return;
              ***REMOVED***

          ***REMOVED***

            string headIconDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HeadIcons");

            // Robin already contains the word "rob" so I'm hardcoding it
            if (!string.Equals(character, "rob") && !string.Equals(character, "samus"))
            ***REMOVED***
                headIconFileName = Directory.GetFiles(headIconDirectory).Where(s => s.ToLower().Contains(character)).ToList();
          ***REMOVED***
            else if (string.Equals(character, "rob"))
            ***REMOVED***
                headIconFileName.Add(string.Concat(headIconDirectory, @"\ROBHeadSSBUWebsite.png"));
          ***REMOVED***             
            else if (string.Equals(character, "samus"))
            ***REMOVED***
                headIconFileName.Add(string.Concat(headIconDirectory, @"\SamusHeadSSBUWebsite.png"));
          ***REMOVED***
                           

            using (var fs = new FileStream(headIconFileName[0], FileMode.Open))
            ***REMOVED***
                var embed = new DiscordEmbedBuilder
                ***REMOVED***
                    Title = $"Dr. Mario vs. ***REMOVED***advice.Title***REMOVED***",
                    Url = docStop,
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    ***REMOVED***
                        IconUrl = docHeadUrl,
                        Name = author
                  ***REMOVED***,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    ***REMOVED***
                        IconUrl = docHeadUrl,
                        Text = footer
                  ***REMOVED***,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    ***REMOVED***
                        Url = $"attachment://***REMOVED***Path.GetFileName(headIconFileName[0])***REMOVED***"
                  ***REMOVED***,

                    Color = new Optional<DiscordColor>(new DiscordColor(advice.Color)),
              ***REMOVED***;

                if (!string.IsNullOrEmpty(advice.SecondaryNotes))
                ***REMOVED***
                    string[] notesTitles = ReturnNotesTitle(advice);

                    embed.AddField(notesTitles[0], BuildNotes(advice));

                    embed.AddField(notesTitles[1], string.Concat("```\n", advice.SecondaryNotes, "```"));

                    if (!string.IsNullOrEmpty(advice.TertiaryNotes))
                        embed.AddField(notesTitles[2], string.Concat("```\n", advice.TertiaryNotes, "```"));
              ***REMOVED***
                else
                ***REMOVED***
                    embed.AddField("Notes", BuildNotes(advice));
              ***REMOVED***

                embed.AddField("Stages (ordered from best to worst)", BuildStages(advice));
                embed.AddField("Documents", BuildOtherFields(advice.Document));
                embed.AddField("Videos", BuildOtherFields(advice.Video));
                embed.AddField("Other", BuildOtherFields(advice.Other));

                var msg = await new DiscordMessageBuilder()
                        .WithEmbed(embed)
                        .WithFiles(new Dictionary<string, Stream>() ***REMOVED*** ***REMOVED*** $"***REMOVED***Path.GetFileName(headIconFileName[0])***REMOVED***", fs ***REMOVED*** ***REMOVED***)
                        .SendAsync(ctx.Channel);
          ***REMOVED***

      ***REMOVED***

        private static string BuildNotes(MatchupAdvice notes)
        ***REMOVED***
            var sb = new StringBuilder("```");

            if (!string.IsNullOrEmpty(notes.PrimaryNotes))
            ***REMOVED***
                sb.Append(notes.PrimaryNotes);
          ***REMOVED***
            else
            ***REMOVED***
                return "---WORK IN PROGRESS---";
          ***REMOVED***

            sb.Append("```");

            return sb.ToString();
      ***REMOVED***

        private static string BuildStages(MatchupAdvice stages)
        ***REMOVED***
            if (!string.IsNullOrEmpty(stages.WinningStages))
            ***REMOVED***
                return string.Concat("```", stages.WinningStages, "\n", stages.NeutralStages, stages.LosingStages, "```");
          ***REMOVED***
            else
            ***REMOVED***
                return "```/```";
          ***REMOVED***
      ***REMOVED***

        private static string BuildOtherFields(string txt)
        ***REMOVED***
            if (!string.IsNullOrEmpty(txt))
            ***REMOVED***
                return txt;
          ***REMOVED***
            else
            ***REMOVED***
                return "/";
          ***REMOVED***
      ***REMOVED***

        private static string[] ReturnNotesTitle(MatchupAdvice advice)
        ***REMOVED***
            switch (advice.Title)
            ***REMOVED***
                case "(Dark) Pit":
                    return new string[] ***REMOVED*** "Pit", "Dark Pit" ***REMOVED***;

                case "Pokémon Trainer":
                    return new string[] ***REMOVED*** "Squirtle", "Ivysaur", "Charizard" ***REMOVED***;

                case "Ryu":
                    return new string[] ***REMOVED*** "Ryu", "Ken" ***REMOVED***;

                default:
                    return new string[] ***REMOVED*** " -Unknown " ***REMOVED***;
          ***REMOVED***
      ***REMOVED***
  ***REMOVED***

    public class MatchupAdvice
    ***REMOVED***
        public string Character ***REMOVED*** get; set; ***REMOVED***
        public string Color ***REMOVED*** get; set; ***REMOVED***
        public string Document ***REMOVED*** get; set; ***REMOVED***
        public string Video ***REMOVED*** get; set; ***REMOVED***
        public string WinningStages ***REMOVED*** get; set; ***REMOVED***
        public string NeutralStages ***REMOVED*** get; set; ***REMOVED***
        public string LosingStages ***REMOVED*** get; set; ***REMOVED***
        public string Other ***REMOVED*** get; set; ***REMOVED***
        public string PrimaryNotes ***REMOVED*** get; set; ***REMOVED***
        public string SecondaryNotes ***REMOVED*** get; set; ***REMOVED***
        public string TertiaryNotes ***REMOVED*** get; set; ***REMOVED***
        public string Title ***REMOVED*** get; set; ***REMOVED***
  ***REMOVED***

    public class Aliases
    ***REMOVED***
        public string Alias ***REMOVED*** get; set; ***REMOVED***
        public string Character ***REMOVED*** get; set; ***REMOVED***
  ***REMOVED***
***REMOVED***