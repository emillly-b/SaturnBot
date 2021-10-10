using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using SaturnBot.Services;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;

namespace SaturnBot.Modules
{
    [Remarks("Public Commands")]
    public class PublicModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider Services { get; set; }
        public CommandService Commands { get; set; }

        [Command("help")]
        [Remarks("Lists all commands and general information about Saturn.")]
        public async Task HelpAsync()
        {
            var commandList = Commands.Modules.ToList();
            var builder = new EmbedBuilder()
            {
                Color = Color.Blue,
                Title = "Help Screen",
                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                Description = "Saturn Bot: its wiggy"
            };
            builder.WithCurrentTimestamp();
            builder.AddField("Command Count:", Commands.Commands.ToList().Count);
            foreach (ModuleInfo info in commandList)
            {
                string commands = "";
                foreach(CommandInfo command in info.Commands.ToList())
                {
                    commands += $"`{command.Name}` - {command.Remarks}\r\n";
                }
                builder.AddField(info.Remarks, "\r\n" + commands);
            }
            var helpEmbed = builder.Build();
            await ReplyAsync("", embed: helpEmbed);
        }

        [Command("info")]
        [Remarks("Lists general information about Saturn.")]
        public async Task InfoAsync()
        {
            var builder = new EmbedBuilder()
            {
                Color = Color.DarkRed,
                Title = "Saturn Bot: its wiggy",
                ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl(),
                Description = "Barebones and to the point"
            };
            builder.WithCurrentTimestamp();
            builder.AddField("Github", "https://github.com/emillly-b/SaturnBot");
            await Context.Message.ReplyAsync("", embed: builder.Build());
        }

        [Command("praise")]
        [Remarks("Praises whatever you tell it too!.")]
        public async Task PraiseAsync(string thingToPraise)
        {
            var author = Context.Message.Author.Mention;
            if (MentionUtils.TryParseUser(thingToPraise, out ulong id))
            {
                var mention = Context.Guild.GetUser(id).Mention;
                await ReplyAsync($"{author} has chosen {mention} as their lord and savior, praise {mention}");
            }
            else
            {
                await ReplyAsync($"{author} has chosen {thingToPraise} as their lord and savior, praise {thingToPraise}");
            }
        }

        [Command("spook")]
        [Remarks("Returns the spooky version of the input text.")]
        public async Task SpookAsync(string input)
        {
            var newstring = "";
            foreach(char c in input)
            {
                newstring += c + " ";
            }
            await ReplyAsync(newstring);
        }
    }
}
