using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord;

namespace SaturnBot.Modules
{
    // Remember to make your module reference the ShardedCommandContext
    public class PublicModule : ModuleBase<ShardedCommandContext>
    {
        [Command("info")]
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
        public async Task PraiseAsync(string user)
        {
            var id = MentionUtils.ParseUser(user);
            var author = Context.Message.Author.Mention;
            var mention = Context.Guild.GetUser(id).Mention;
            await ReplyAsync($"{author} has chosen {mention} as their lord and savior, praise {mention}");
        }

        [Command("quiz")]
        public async Task ShitListAsync()
        {
            var chars = new string[,]
            {
                {"Max", "https://decider.com/wp-content/uploads/2019/06/stranger-things-max.jpg?quality=90&strip=all&w=646&h=431&crop=1"},
                { "Dustin", "https://static.wikia.nocookie.net/strangerthings8338/images/1/18/Dustin_S2.png/revision/latest?cb=20180319174421"},
                { "Eleven",  "https://media.vanityfair.com/photos/59f370dd16ff751cf425ef46/master/pass/wv_publicity_pre_launch_A_still_23.jpg"},
                { "Billy", "https://adultpaintbynumber.com/wp-content/uploads/2020/10/Billy-Hargrove-paint-by-numbers.jpg" },
                { "Billy", "https://www.pinclipart.com/picdir/middle/91-914392_billy-png-clipart-billy-y-mandy-render-transparent.png" }
            };
            var rand = new Random();
            var index = rand.Next(4);
            var builder = new EmbedBuilder()
                .WithTitle($"You are {chars[index, 0]}!")
                .WithImageUrl($"{chars[index, 1]}")
                .WithFooter("List Credit: WickedCat!");
            var embed = builder.Build();
            await Context.Message.ReplyAsync("", embed: embed);
        }
    }
}
