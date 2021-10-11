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
    public class ReactionHandlingService
    {
        private DiscordShardedClient _discord;
        private IServiceProvider _services;
        public List<Starboard> ActiveStarboards;

        public ReactionHandlingService(IServiceProvider services)
        {
            _services = services;
        }
        public void Initialize()
        {
            var client = _services.GetRequiredService<DiscordShardedClient>();
            var guilds = _services.GetRequiredService<GuildHandlingService>();
            foreach(Guild guild in guilds.ActiveGuilds)
            {
                try
                {
                    if (guild.Starboard.IsEnabled)
                        ActiveStarboards.Add(guild.Starboard);
                }
                catch
                {
                    guild.Starboard = new Starboard();
                }
            }
            client.ReactionAdded += ReactionAdded;
        }
        public async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> messageChannel, SocketReaction reaction)
        {
            //if (reaction.Emote.Name.Equals("⭐"))
            //    Console.WriteLine("Star Recieved!");
        }
    }
}