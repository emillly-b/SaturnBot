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
    public class OwnerModule : ModuleBase<ShardedCommandContext>
    {
        public IServiceProvider Services { get; set; }

        [Command("botstatus")]
        [RequireOwner]
        public async Task UpdateStatus(string status)
        {
            await Services.GetRequiredService<DiscordShardedClient>().SetGameAsync(status);
            await ReplyAsync($"Bot status has been set to: {status}");
        }
        [Command("savedb")]
        [RequireOwner]
        public async Task SaveDB()
        {
            await Services.GetRequiredService<GuildHandlingService>().SaveGuildsAsync();
            await ReplyAsync($"Database Saved");
        }
    }
}
