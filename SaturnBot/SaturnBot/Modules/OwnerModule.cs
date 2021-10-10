using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SaturnBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SaturnBot.Modules
{
    [Remarks("Bot Owner Commands")]
    public class OwnerModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider Services { get; set; }

        [Command("botstatus")]
        [Remarks("Sets the bot status.")]
        [RequireOwner]
        public async Task UpdateStatus(string status)
        {
            await Services.GetRequiredService<DiscordShardedClient>().SetGameAsync(status);
            await ReplyAsync($"Bot status has been set to: {status}");
        }
        [Command("savedb")]
        [Remarks("Manually saves the database.")]
        [RequireOwner]
        public async Task SaveDB()
        {
            await Services.GetRequiredService<GuildHandlingService>().SaveGuildsAsync();
            await ReplyAsync($"Database Saved");
        }

        [Command("mimic")]
        [Remarks("Responds with what was said")]
        [RequireOwner]
        public async Task MimicAsync(string input)
        {
            ReplyAsync(input);
            await Context.Message.DeleteAsync();
        }
    }
}
