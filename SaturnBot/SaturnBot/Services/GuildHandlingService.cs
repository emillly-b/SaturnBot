using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SaturnBot.Services
{
    public class GuildHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordShardedClient _discord;
        private readonly IServiceProvider _services;

        public GuildHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordShardedClient>();
            _services = services;

            _discord.GuildAvailable += GuildAvailable;
            
        }
        private Task GuildAvailable(SocketGuild arg)
        {
            throw new NotImplementedException();
        }
    }
}
