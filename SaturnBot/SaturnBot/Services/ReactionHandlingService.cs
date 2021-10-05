using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SaturnBot.Services
{
    public class ReactionHandlingService
    {
        private DiscordShardedClient _discord;
        private IServiceProvider _services;
        public ReactionHandlingService(IServiceProvider services)
        {
            _services = services;
        }
        public void Initialize()
        {
            var client = _services.GetRequiredService<DiscordShardedClient>();
            client.ReactionAdded += ReactionAdded;
        }
        public async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> messageChannel, SocketReaction reaction)
        {
            if (reaction.Emote.Name.Equals("⭐"))
                Console.WriteLine("Star Recieved!");
        }
    }
}