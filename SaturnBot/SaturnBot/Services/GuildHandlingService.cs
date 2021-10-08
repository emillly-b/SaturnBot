using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SaturnBot.Entities;

namespace SaturnBot.Services
{
    public class GuildHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;
        public List<Guild> ActiveGuilds;

        public GuildHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _services = services;

            ActiveGuilds = new List<Guild>();
            _discord.GuildAvailable += GuildAvailable;
            foreach (SocketGuild guild in _discord.Guilds)
                ActiveGuilds.Add(new Guild(guild));
        }
        public Guild GetGuild(ulong id)
        {
            return ActiveGuilds.Find(x => x.Id == id);
        }

        private async Task GuildAvailable(SocketGuild arg)
        {
            ActiveGuilds.Add(new Guild(arg));
        }
        private async Task GuildMemberUpdated(Cacheable<SocketGuildUser,ulong> userBefore, SocketGuildUser userAfter)
        {
            Guild guild = GetGuild(userAfter.Guild.Id);
            var embed = new EmbedBuilder()
                .WithTitle($"User Updated :{userAfter.Username}");
            if(userAfter is not SocketGuildUser)
            {
                embed.AddField("Error", "Unable to determine what has changed.");
                var channel = _discord.GetChannel(guild.LoggingChannelId) as ITextChannel;
                await channel.SendMessageAsync("", embed: embed.Build());
            }
        }
    }
}
