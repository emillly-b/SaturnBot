using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SaturnBot.Services
{
    public class CoreProviderService
    {
        private Core Core { get; set;}

        public CoreProviderService(IServiceProvider services)
        {
            Core = new Core();
        }
        public Core GetCore()
        {
            return Core;
        }
    }
}
