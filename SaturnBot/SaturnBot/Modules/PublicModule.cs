using System.Threading.Tasks;
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
    }
}
