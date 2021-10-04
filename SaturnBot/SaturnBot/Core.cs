using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.API;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SaturnBot.Services;
using SaturnBot.Modules;

namespace SaturnBot
{
    public class Core
    {
        public Configuration Configuration { get; set; }        
        public Core()
        {
            Configuration = Configuration.GetConfiguration("./Data/Config.json");
        }


        public Task ReadyAsync(DiscordSocketClient shard)
        {
            return Task.CompletedTask;
        }
        public Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
